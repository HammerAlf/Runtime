// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Applications;
using Dolittle.Logging;
using Dolittle.Runtime.Events.Store;
using Dolittle.Runtime.Events.Streams;
using Dolittle.Tenancy;

#pragma warning disable CA2008

namespace Dolittle.Runtime.Events.Processing.Streams
{
    /// <summary>
    /// Represents a system that processes a stream of events.
    /// </summary>
    public class StreamProcessor : IDisposable
    {
        readonly IEventProcessor _processor;
        readonly ILogger _logger;
        readonly IFetchEventsFromStreams _eventsFromStreamsFetcher;
        readonly string _logMessagePrefix;
        readonly CancellationTokenSource _cancellationTokenSource;
        readonly IStreamProcessorStates _streamProcessorStates;
        Task _task;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamProcessor"/> class.
        /// </summary>
        /// <param name="tenantId">The <see cref="TenantId"/> the <see cref="StreamProcessor"/> belongs to.</param>
        /// <param name="sourceStreamId">The <see cref="StreamId" /> of the source stream.</param>
        /// <param name="sourceMicroservice">The source <see cref="Microservice" />.</param>
        /// <param name="processor">An <see cref="IEventProcessor" /> to process the event.</param>
        /// <param name="streamProcessorStates">The <see cref="IStreamProcessorStates" />.</param>
        /// <param name="eventsFromStreamsFetcher">The<see cref="IFetchEventsFromStreams" />.</param>
        /// <param name="cancellationTokenSource">The <see cref="CancellationTokenSource" />.</param>
        /// <param name="logger">An <see cref="ILogger" /> to log messages.</param>
        public StreamProcessor(
            TenantId tenantId,
            StreamId sourceStreamId,
            Microservice sourceMicroservice,
            IEventProcessor processor,
            IStreamProcessorStates streamProcessorStates,
            IFetchEventsFromStreams eventsFromStreamsFetcher,
            CancellationTokenSource cancellationTokenSource,
            ILogger logger)
        {
            _processor = processor;
            _logger = logger;
            _eventsFromStreamsFetcher = eventsFromStreamsFetcher;
            Identifier = new StreamProcessorId(_processor.Identifier, sourceStreamId, sourceMicroservice);
            _logMessagePrefix = $"Stream Partition Processor for event processor '{Identifier.EventProcessorId}' with source stream '{Identifier.SourceStreamId}' for tenant '{tenantId}'";
            CurrentState = StreamProcessorState.New;
            _cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();
            _streamProcessorStates = streamProcessorStates;
        }

        /// <summary>
        /// Gets the <see cref="StreamProcessorId">identifier</see> for the <see cref="StreamProcessor"/>.
        /// </summary>
        public StreamProcessorId Identifier { get; }

        /// <summary>
        /// Gets the <see cref="EventProcessorId" />.
        /// </summary>
        public EventProcessorId EventProcessorId => _processor.Identifier;

        /// <summary>
        /// Gets the current <see cref="StreamProcessorState" />.
        /// </summary>
        public StreamProcessorState CurrentState { get; private set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }

        /// <summary>
        /// Start processing.
        /// </summary>
        public void Start()
        {
            _task = Task.Factory.StartNew(BeginProcessing, TaskCreationOptions.DenyChildAttach);
        }

        /// <summary>
        /// Stop processing.
        /// </summary>
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Starts up the <see cref="StreamProcessor" />.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task BeginProcessing()
        {
            try
            {
                CurrentState = await _streamProcessorStates.GetStoredStateFor(Identifier, _cancellationTokenSource.Token).ConfigureAwait(false);
                do
                {
                    StreamEvent streamEvent = default;
                    while (streamEvent == default && !_cancellationTokenSource.IsCancellationRequested)
                    {
                        try
                        {
                            CurrentState = await _streamProcessorStates.FailingPartitions.CatchupFor(Identifier, _processor, CurrentState, _cancellationTokenSource.Token).ConfigureAwait(false);
                            streamEvent = await FetchNextEventWithPartitionToProcess().ConfigureAwait(false);
                        }
                        catch (NoEventInStreamAtPosition)
                        {
                            await Task.Delay(250).ConfigureAwait(false);
                        }
                        catch (EventStoreUnavailable)
                        {
                            await Task.Delay(1000).ConfigureAwait(false);
                        }
                    }

                    if (_cancellationTokenSource.IsCancellationRequested) break;

                    CurrentState = await _streamProcessorStates.ProcessEventAndChangeStateFor(Identifier, _processor, streamEvent, CurrentState, _cancellationTokenSource.Token).ConfigureAwait(false);
                }
                while (!_cancellationTokenSource.IsCancellationRequested);
            }
            catch (Exception ex)
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _logger.Error($"{_logMessagePrefix} failed - {ex}");
                }
            }
        }

        Task<StreamEvent> FetchNextEventWithPartitionToProcess()
        {
            _logger.Debug($"{_logMessagePrefix} is fetching event at position '{CurrentState.Position}'.");
            return _eventsFromStreamsFetcher.Fetch(Identifier.SourceStreamId, Identifier.SourceMicroservice, CurrentState.Position, _cancellationTokenSource.Token);
        }
    }
}