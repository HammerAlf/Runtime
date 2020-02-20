// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
extern alias contracts;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using contracts::Dolittle.Runtime.Events.Processing;
using Dolittle.Applications;
using Dolittle.Collections;
using Dolittle.DependencyInversion;
using Dolittle.Execution;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Protobuf;
using Dolittle.Runtime.Events.Processing.Streams;
using Dolittle.Runtime.Events.Store;
using Dolittle.Tenancy;
using grpc = contracts::Dolittle.Runtime.Events.Processing;

namespace Dolittle.Runtime.Events.Processing.EventHorizon
{
    /// <summary>
    /// Represents an implementation of <see cref="IEventHorizonClient" />.
    /// </summary>
    [Singleton]
    public class EventHorizonClient : IEventHorizonClient
    {
        readonly EventHorizonsConfiguration _eventHorizons;
        readonly grpc.EventHorizon.EventHorizonClient _client;
        readonly IExecutionContextManager _executionContextManager;
        readonly FactoryFor<StreamProcessors> _getStreamProcessors;
        readonly FactoryFor<IStreamProcessorStateRepository> _getStreamProcessorStates;
        readonly FactoryFor<IWriteReceivedEvents> _getReceivedEventsWriter;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHorizonClient"/> class.
        /// </summary>
        /// <param name="eventHorizons">The <see cref="EventHorizonsConfiguration" />.</param>
        /// <param name="client">The grpc client.</param>
        /// <param name="executionContextManager">The <see cref="IExecutionContextManager" />.</param>
        /// <param name="getStreamProcessors">The <see cref="FactoryFor{IStreamProcessors}" />.</param>
        /// <param name="getStreamProcessorStates">The <see cref="FactoryFor{IStreamProcessorStateRepository}" />.</param>
        /// <param name="getReceivedEventsWriter">The <see cref="FactoryFor{IWriteReceivedEvents}" />.</param>
        /// <param name="logger">The <see cref="ILogger" />.</param>
        public EventHorizonClient(
            EventHorizonsConfiguration eventHorizons,
            grpc.EventHorizon.EventHorizonClient client,
            IExecutionContextManager executionContextManager,
            FactoryFor<StreamProcessors> getStreamProcessors,
            FactoryFor<IStreamProcessorStateRepository> getStreamProcessorStates,
            FactoryFor<IWriteReceivedEvents> getReceivedEventsWriter,
            ILogger logger)
        {
            _eventHorizons = eventHorizons;
            _client = client;
            _executionContextManager = executionContextManager;
            _getStreamProcessors = getStreamProcessors;
            _getStreamProcessorStates = getStreamProcessorStates;
            _getReceivedEventsWriter = getReceivedEventsWriter;
            _logger = logger;
        }

        /// <inheritdoc/>
        public void Subscribe()
        {
            _eventHorizons.ForEach(_ => SubscribeToMicroService(_.Key, _.Value));
        }

        /// <inheritdoc/>
        public async Task StartSubscription(Microservice microservice, TenantId producer, TenantId subscriber)
        {
            StreamProcessor streamProcessor = null;
            var tokenSource = new CancellationTokenSource();
            try
            {
                _executionContextManager.CurrentFor(subscriber);
                var publicEventsVersion = (await _getStreamProcessorStates().GetOrAddNew(new StreamProcessorId(producer.Value, microservice.Value)).ConfigureAwait(false)).Position;
                var eventsFetcher = new EventsFromEventHorizonFetcher(
                    _client.Subscribe(
                        new EventHorizonSubscriberToPublisherRequest
                        {
                            Microservice = _executionContextManager.Current.BoundedContext.Value.ToProtobuf(),
                            ProducerTenant = producer.ToProtobuf(),
                            SubscriberTenant = subscriber.ToProtobuf(),
                            PublicEventsVersion = publicEventsVersion.Value
                        },
                        cancellationToken: tokenSource.Token),
                    publicEventsVersion);

                streamProcessor = _getStreamProcessors().Register(
                    new ReceivedEventsProcessor(microservice, producer, _getReceivedEventsWriter(), _logger),
                    eventsFetcher,
                    microservice.Value,
                    tokenSource);

                while (!tokenSource.IsCancellationRequested)
                {
                    await Task.Delay(50).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                _logger.Warning($"DIsconnecting Event Horizon to microservice '{microservice}' and tenant '{producer}'");
            }
            finally
            {
                tokenSource.Dispose();
                streamProcessor?.Dispose();
            }
        }

        void SubscribeToMicroService(Microservice microservice, Subscriptions subscriptions) => subscriptions.ForEach(_ => SubscribeToTenantInMicroService(microservice, _.Key, _.Value));

        void SubscribeToTenantInMicroService(Microservice microservice, TenantId tenant, IEnumerable<TenantId> subscribers) => subscribers.ForEach(subscriber =>
        {
            var task = StartSubscription(microservice, tenant, subscriber);
        });
    }
}