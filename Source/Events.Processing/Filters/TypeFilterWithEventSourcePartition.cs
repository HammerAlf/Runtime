// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

extern alias contracts;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Applications;
using Dolittle.Logging;
using Dolittle.Runtime.Events.Store;
using Dolittle.Runtime.Events.Streams;

namespace Dolittle.Runtime.Events.Processing.Filters
{
    /// <summary>
    /// Represents a <see cref="AbstractFilterProcessor{T}"/> that filters by known event types and can partition using an <see cref="EventSourceId"/>.
    /// </summary>
    public class TypeFilterWithEventSourcePartition : AbstractFilterProcessor<TypeFilterWithEventSourcePartitionDefinition>
    {
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeFilterWithEventSourcePartition"/> class.
        /// </summary>
        /// <param name="sourceMicroservice">The source <see cref="Microservice" />.</param>
        /// <param name="definition">The<see cref="TypeFilterWithEventSourcePartitionDefinition"/>.</param>
        /// <param name="eventsToStreamsWriter">The <see cref="IWriteEventsToStreams">writer</see> for writing events.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public TypeFilterWithEventSourcePartition(
            Microservice sourceMicroservice,
            TypeFilterWithEventSourcePartitionDefinition definition,
            IWriteEventsToStreams eventsToStreamsWriter,
            ILogger logger)
            : base(sourceMicroservice, definition, eventsToStreamsWriter, logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public override async Task<IFilterResult> Filter(CommittedEvent @event, PartitionId partitionId, EventProcessorId eventProcessorId, CancellationToken cancellationToken)
        {
            try
            {
                var included = Definition.Types.Contains(@event.Type.Id);
                var outPartitionId = PartitionId.NotSet;
                if (Definition.Partitioned)
                {
                    outPartitionId = @event.EventSource.Value;
                }

                var filterResult = new SucceededFilteringResult(included, outPartitionId);
                return await Task.FromResult(filterResult).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new FailedFilteringResult($"Failure Message: {ex.Message}\nStack Trace: {ex.StackTrace}")).ConfigureAwait(false);
            }
        }
    }
}