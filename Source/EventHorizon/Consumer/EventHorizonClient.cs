// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
extern alias contracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using contracts::Dolittle.Runtime.Events.Processing;
using Dolittle.Applications.Configuration;
using Dolittle.DependencyInversion;
using Dolittle.Execution;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Protobuf;
using Dolittle.Runtime.Events.Processing.Streams;
using Dolittle.Runtime.Events.Store;
using Dolittle.Services.Clients;
using Dolittle.Tenancy;
using grpc = contracts::Dolittle.Runtime.Events.Processing;

namespace Dolittle.Runtime.EventHorizon.Consumer
{
    /// <summary>
    /// Represents an implementation of <see cref="IEventHorizonClient" />.
    /// </summary>
    [Singleton]
    public class EventHorizonClient : IEventHorizonClient
    {
        readonly EventHorizonsConfiguration _eventHorizons;
        readonly IEventHorizonSubscriptions _eventHorizonSubscriptions;
        readonly BoundedContextConfiguration _boundedContextConfiguration;
        readonly IClientManager _clientManager;
        readonly IExecutionContextManager _executionContextManager;
        readonly FactoryFor<StreamProcessors> _getStreamProcessors;
        readonly FactoryFor<IStreamProcessorStateRepository> _getStreamProcessorStates;
        readonly FactoryFor<IWriteReceivedEvents> _getReceivedEventsWriter;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHorizonClient"/> class.
        /// </summary>
        /// <param name="eventHorizons">The <see cref="EventHorizonsConfiguration" />.</param>
        /// <param name="boundedContextConfiguration">The <see cref="BoundedContextConfiguration" />.</param>
        /// <param name="eventHorizonSubscriptions">The <see cref="IEventHorizonSubscriptions" />.</param>
        /// <param name="clientManager">The <see cref="IClientManager" />.</param>
        /// <param name="executionContextManager">The <see cref="IExecutionContextManager" />.</param>
        /// <param name="getStreamProcessors">The <see cref="FactoryFor{IStreamProcessors}" />.</param>
        /// <param name="getStreamProcessorStates">The <see cref="FactoryFor{IStreamProcessorStateRepository}" />.</param>
        /// <param name="getReceivedEventsWriter">The <see cref="FactoryFor{IWriteReceivedEvents}" />.</param>
        /// <param name="logger">The <see cref="ILogger" />.</param>
        public EventHorizonClient(
            EventHorizonsConfiguration eventHorizons,
            BoundedContextConfiguration boundedContextConfiguration,
            IEventHorizonSubscriptions eventHorizonSubscriptions,
            IClientManager clientManager,
            IExecutionContextManager executionContextManager,
            FactoryFor<StreamProcessors> getStreamProcessors,
            FactoryFor<IStreamProcessorStateRepository> getStreamProcessorStates,
            FactoryFor<IWriteReceivedEvents> getReceivedEventsWriter,
            ILogger logger)
        {
            _eventHorizons = eventHorizons;
            _eventHorizonSubscriptions = eventHorizonSubscriptions;
            _boundedContextConfiguration = boundedContextConfiguration;
            _clientManager = clientManager;
            _executionContextManager = executionContextManager;
            _getStreamProcessors = getStreamProcessors;
            _getStreamProcessorStates = getStreamProcessorStates;
            _getReceivedEventsWriter = getReceivedEventsWriter;
            _logger = logger;
        }

        /// <inheritdoc/>
        public void Subscribe()
        {
            var subscriptionsPerTenant = new Dictionary<TenantId, IEnumerable<EventHorizonSubscription>>();
            foreach ((var subscriber, var eventHorizons) in _eventHorizons)
            {
                var subscriptions = eventHorizons.Select(_ => _eventHorizonSubscriptions.GetSubscriptionFor(subscriber, _.Microservice, _.Tenant));
                subscriptionsPerTenant.Add(subscriber, subscriptions);
            }

            foreach ((var subscriber, var subscriptions) in subscriptionsPerTenant)
            {
                subscriptions.Select(_ => StartSubscription(subscriber, _));
            }
        }

        /// <inheritdoc/>
        public async Task StartSubscription(TenantId subscriber, EventHorizonSubscription subscription)
        {
            while (true)
            {
                var producer = subscription.Tenant;
                var microservice = subscription.Microservice;
                var streamProcessorId = new StreamProcessorId(producer.Value, microservice.Value);
                _logger.Debug($"Tenant '{subscriber}' is subscribing to events from tenant '{producer} in microservice '{microservice}' on '{subscription.Host}:{subscription.Port}'");
                try
                {
#pragma warning disable CA2000
                    var tokenSource = new CancellationTokenSource();
                    _executionContextManager.CurrentFor(subscriber);
                    var publicEventsPosition = (await _getStreamProcessorStates().GetOrAddNew(streamProcessorId).ConfigureAwait(false)).Position;
                    var eventsFetcher = new EventsFromEventHorizonFetcher(
                        _clientManager.Get<grpc.EventHorizon.EventHorizonClient>(subscription.Host, subscription.Port).Subscribe(
                            new EventHorizonSubscriberToPublisherRequest
                            {
                                Microservice = _boundedContextConfiguration.BoundedContext.Value.ToProtobuf(),
                                ProducerTenant = producer.ToProtobuf(),
                                SubscriberTenant = subscriber.ToProtobuf(),
                                PublicEventsPosition = publicEventsPosition.Value
                            },
                            cancellationToken: tokenSource.Token),
                        () =>
                        {
                            if (!tokenSource.IsCancellationRequested)
                            {
                                _logger.Debug($"Canceling cancellation token source for Event Horizon from tenant '{subscriber}' to tenant '{producer}' in microservice '{microservice}'");
                                tokenSource.Cancel();
                            }
                        },
                        _logger);
                    _getStreamProcessors().Register(
                        new ReceivedEventsProcessor(microservice, producer, _getReceivedEventsWriter(), _logger),
                        eventsFetcher,
                        microservice.Value,
                        tokenSource);
#pragma warning restore CA2000

                    while (!tokenSource.IsCancellationRequested)
                    {
                        await Task.Delay(5000).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Error occurred while handling Event Horizon to microservice '{microservice}' and tenant '{producer}'");
                }
                finally
                {
                    _logger.Debug($"Disconnecting Event Horizon from tenant '{subscriber}' to microservice '{microservice}' and tenant '{producer}'");
                    _executionContextManager.CurrentFor(subscriber);
                    _getStreamProcessors().Unregister(streamProcessorId.EventProcessorId, streamProcessorId.SourceStreamId);
                }

                await Task.Delay(5000).ConfigureAwait(false);
            }
        }
    }
}