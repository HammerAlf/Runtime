// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

extern alias contracts;

using System.Threading;
using System.Threading.Tasks;
using contracts::Dolittle.Runtime.Events.Processing;
using Dolittle.Execution;
using Dolittle.Logging;
using Dolittle.Protobuf;
using Dolittle.Runtime.Events.Store;
using Dolittle.Runtime.Events.Streams;
using Dolittle.Services;

namespace Dolittle.Runtime.Events.Processing.EventHorizon
{
    /// <summary>
    /// Represents an implementation of <see cref="IEventProcessor" />that processes the handling of an event.
    /// </summary>
    public class ExternalEventProcessor : IEventProcessor
    {
        readonly IReverseCallDispatcher<ExternalEventHandlerClientToRuntimeResponse, ExternalEventHandlerRuntimeToClientRequest> _callDispatcher;
        readonly IExecutionContextManager _executionContextManager;
        readonly ILogger _logger;
        readonly string _logMessagePrefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalEventProcessor"/> class.
        /// </summary>
        /// <param name="id">The <see cref="EventProcessorId" />.</param>
        /// <param name="callDispatcher"><see cref="IReverseCallDispatcher{TResponse, TRequest}"/> for server requests.</param>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for current <see cref="Execution.ExecutionContext"/>.</param>
        /// <param name="logger">The <see cref="ILogger" />.</param>
        public ExternalEventProcessor(
            EventProcessorId id,
            IReverseCallDispatcher<ExternalEventHandlerClientToRuntimeResponse, ExternalEventHandlerRuntimeToClientRequest> callDispatcher,
            IExecutionContextManager executionContextManager,
            ILogger logger)
        {
            _callDispatcher = callDispatcher;
            _executionContextManager = executionContextManager;
            Identifier = id;
            _logger = logger;
            _logMessagePrefix = $"Event Processor '{Identifier}'";
        }

        /// <inheritdoc />
        public EventProcessorId Identifier { get; }

        /// <inheritdoc />
        public async Task<IProcessingResult> Process(CommittedEvent @event, PartitionId partitionId, CancellationToken cancellationToken = default)
        {
            _logger.Debug($"{_logMessagePrefix} is processing event '{@event.Type.Id.Value}' for partition '{partitionId.Value}'");

            var message = new ExternalEventHandlerRuntimeToClientRequest
            {
                Event = @event.ToProtobuf(),
                Partition = partitionId.ToProtobuf(),
                ExecutionContext = _executionContextManager.Current.ToByteString()
            };
            IProcessingResult result = null;
            await _callDispatcher.Call(message, response => result = response.ToProcessingResult()).ConfigureAwait(false);

            return result;
        }
    }
}