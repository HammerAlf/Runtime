// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

extern alias contracts;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using contracts::Dolittle.Runtime.Events.Processing;
using Dolittle.Applications;
using Dolittle.Artifacts;
using Dolittle.Collections;
using Dolittle.DependencyInversion;
using Dolittle.Execution;
using Dolittle.Logging;
using Dolittle.Protobuf;
using Dolittle.Runtime.Events.Processing.Filters;
using Dolittle.Runtime.Events.Processing.Streams;
using Dolittle.Runtime.Events.Streams;
using Dolittle.Runtime.Tenancy;
using Dolittle.Services;
using Grpc.Core;
using static contracts::Dolittle.Runtime.Events.Processing.ExternalEventHandlers;

namespace Dolittle.Runtime.Events.Processing.EventHorizon
{
    /// <summary>
    /// Represents the implementation of <see cref="ExternalEventHandlersBase"/>.
    /// </summary>
    public class ExternalEventHandlersService : ExternalEventHandlersBase
    {
        readonly IExecutionContextManager _executionContextManager;
        readonly ITenants _tenants;
        readonly FactoryFor<IFilterRegistry> _getFilters;
        readonly FactoryFor<IStreamProcessors> _streamProcessorsFactory;
        readonly FactoryFor<IWriteEventsToStreams> _eventsToStreamsWriterFactory;
        readonly FactoryFor<IFetchEventsFromStreams> _eventsFromStreamsFetcherFactory;
        readonly IReverseCallDispatchers _reverseCallDispatchers;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalEventHandlersService"/> class.
        /// </summary>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for current <see cref="Execution.ExecutionContext"/>.</param>
        /// <param name="tenants">The <see cref="ITenants"/> system.</param>
        /// <param name="getFilters">The <see cref="FactoryFor{T}"/> the <see cref="IFilterRegistry" />.</param>
        /// <param name="streamProcessorsFactory"><see cref="FactoryFor{T}"/> the <see cref="IStreamProcessors"/> for registration management.</param>
        /// <param name="eventsToStreamsWriterFactory"><see cref="FactoryFor{T}"/> the  <see cref="IWriteEventsToStreams">writer</see> for writing events.</param>
        /// <param name="eventsFromStreamsFetcherFactory"><see cref="FactoryFor{T}"/> the  <see cref="IFetchEventsFromStreams">fetcher</see> for writing events.</param>
        /// <param name="reverseCallDispatchers">The <see cref="IReverseCallDispatchers"/> for working with reverse calls.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public ExternalEventHandlersService(
            IExecutionContextManager executionContextManager,
            ITenants tenants,
            FactoryFor<IFilterRegistry> getFilters,
            FactoryFor<IStreamProcessors> streamProcessorsFactory,
            FactoryFor<IWriteEventsToStreams> eventsToStreamsWriterFactory,
            FactoryFor<IFetchEventsFromStreams> eventsFromStreamsFetcherFactory,
            IReverseCallDispatchers reverseCallDispatchers,
            ILogger logger)
        {
            _executionContextManager = executionContextManager;
            _tenants = tenants;
            _getFilters = getFilters;
            _streamProcessorsFactory = streamProcessorsFactory;
            _eventsToStreamsWriterFactory = eventsToStreamsWriterFactory;
            _eventsFromStreamsFetcherFactory = eventsFromStreamsFetcherFactory;
            _reverseCallDispatchers = reverseCallDispatchers;
            _logger = logger;
        }

        /// <inheritdoc/>
        public override async Task Connect(
            IAsyncStreamReader<ExternalEventHandlerClientToRuntimeResponse> runtimeStream,
            IServerStreamWriter<ExternalEventHandlerRuntimeToClientRequest> clientStream,
            ServerCallContext context)
        {
            EventProcessorId eventProcessorId = Guid.Empty;
            var sourceStream = StreamId.AllStreamId;
            Microservice sourceMicroservice = Guid.Empty;

            try
            {
                var eventHandlerArguments = context.GetArgumentsMessage<ExternalEventHandlerArguments>();
                eventProcessorId = eventHandlerArguments.EventHandler.To<EventProcessorId>();
                sourceMicroservice = eventHandlerArguments.Microservice.To<Microservice>();
                _logger.Debug($"ExternalEventHandler client connected with id '{eventProcessorId}' for stream '{sourceStream}' on microservice '{sourceMicroservice}'");
                var targetStream = new StreamId { Value = eventProcessorId };
                ThrowIfIllegalTargetStream(targetStream);
                ThrowIfIllegalMicroservice(sourceMicroservice);

                var dispatcher = _reverseCallDispatchers.GetDispatcherFor(
                    runtimeStream,
                    clientStream,
                    context,
                    _ => _.CallNumber,
                    _ => _.CallNumber);
                var filterDefinition = new TypeFilterWithEventSourcePartitionDefinition(
                    sourceStream,
                    targetStream,
                    eventHandlerArguments.Types_.Select(_ => _.Id.To<ArtifactId>()),
                    eventHandlerArguments.Partitioned);
                await RegisterForAllTenants(filterDefinition, dispatcher, eventProcessorId, sourceStream, targetStream, sourceMicroservice, context.CancellationToken).ConfigureAwait(false);
                await dispatcher.WaitTillDisconnected().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (!context.CancellationToken.IsCancellationRequested)
                {
                    _logger.Error(ex, $"Error occurred while processing event handler '{eventProcessorId}'");
                }
            }
            finally
            {
                UnregisterForAllTenants(eventProcessorId, sourceStream);
                _logger.Debug($"EventHandler client disconnected for '{eventProcessorId}'");
            }
        }

        async Task RegisterForAllTenants(
            TypeFilterWithEventSourcePartitionDefinition filterDefinition,
            IReverseCallDispatcher<ExternalEventHandlerClientToRuntimeResponse, ExternalEventHandlerRuntimeToClientRequest> callDispatcher,
            EventProcessorId eventProcessorId,
            StreamId sourceStreamId,
            StreamId targetStreamId,
            Microservice sourceMicroservice,
            CancellationToken cancellationToken)
        {
            _logger.Debug($"Registering event handler '{eventProcessorId}' for stream '{sourceStreamId}' for {_tenants.All.Count()} tenants - types : '{string.Join(",", filterDefinition.Types)}'");
            foreach (var tenant in _tenants.All)
            {
                try
                {
                    _executionContextManager.CurrentFor(tenant);
                    var filter = new TypeFilterWithEventSourcePartition(
                                        sourceMicroservice,
                                        filterDefinition,
                                        _eventsToStreamsWriterFactory(),
                                        _logger);
                    await _getFilters().Register(filter, cancellationToken).ConfigureAwait(false);

                    _streamProcessorsFactory().Register(filter, _eventsFromStreamsFetcherFactory(), sourceStreamId, Guid.Empty);

                    var eventProcessor = new ExternalEventProcessor(
                        eventProcessorId,
                        callDispatcher,
                        _executionContextManager,
                        _logger);

                    _streamProcessorsFactory().Register(eventProcessor, _eventsFromStreamsFetcherFactory(), targetStreamId, Guid.Empty);
                }
                catch (IllegalFilterTransformation ex)
                {
                    _logger.Error(ex, $"The filter for stream '{targetStreamId}' for tenant '{tenant}' does not produce the same stream as the previous filter for that stream. Not registering stream processors.");
                }
            }
        }

        void UnregisterForAllTenants(EventProcessorId eventProcessorId, StreamId sourceStream)
        {
            var tenants = _tenants.All;
            _logger.Debug($"Unregistering filter '{eventProcessorId}' for stream '{sourceStream}' for {tenants.Count()} tenants");
            tenants.ForEach(tenant =>
            {
                _executionContextManager.CurrentFor(tenant);
                _getFilters().Unregister(eventProcessorId.Value);
                _streamProcessorsFactory().Unregister(eventProcessorId, sourceStream, Guid.Empty);
                _streamProcessorsFactory().Unregister(eventProcessorId, eventProcessorId.Value, Guid.Empty);
            });
        }

        void ThrowIfIllegalTargetStream(StreamId stream)
        {
            if (stream.IsNonWriteable) throw new CannotFilterToNonWriteableStream(stream);
        }

        void ThrowIfIllegalMicroservice(Microservice microservice)
        {
            if (microservice.Equals(Guid.Empty)) throw new InvalidMicroserviceForExternalEvents(microservice);
        }
    }
}