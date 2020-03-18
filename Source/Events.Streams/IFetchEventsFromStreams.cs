// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Applications;
using Dolittle.Runtime.Events.Store;

namespace Dolittle.Runtime.Events.Streams
{
    /// <summary>
    /// Defines a system that can fetch <see cref="CommittedEvent">events</see> from <see cref="StreamId">streams</see>.
    /// </summary>
    public interface IFetchEventsFromStreams
    {
        /// <summary>
        /// Fetch the event at a given position in a stream.
        /// </summary>
        /// <param name="streamId"><see cref="StreamId">the stream in the event store</see>.</param>
        /// <param name="microservice">The source <see cref="Microservice" />.</param>
        /// <param name="streamPosition"><see cref="StreamPosition">the position in the stream</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
        /// <returns>The <see cref="StreamEvent" />.</returns>
        Task<StreamEvent> Fetch(StreamId streamId, Microservice microservice, StreamPosition streamPosition, CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetch a range of events from a position to another in a stream.
        /// </summary>
        /// <param name="streamId"><see cref="StreamId">the stream in the event store</see>.</param>
        /// <param name="microservice">The source <see cref="Microservice" />.</param>
        /// <param name="range">The <see cref="StreamPositionRange" />.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
        /// <returns>The <see cref="IEnumerable{T}" /> of <see cref="StreamEvent" />.</returns>
        Task<IEnumerable<StreamEvent>> FetchRange(StreamId streamId, Microservice microservice, StreamPositionRange range, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds the <see cref="StreamPosition" /> of the next event to process in a <see cref="StreamId" /> for a <see cref="PartitionId" />.
        /// </summary>
        /// <param name="streamId">The <see cref="StreamId" />.</param>
        /// <param name="partitionId">The <see cref="PartitionId" />.</param>
        /// <param name="microservice">The source <see cref="Microservice" />.</param>
        /// <param name="fromPosition">The <see cref="StreamPosition" />.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
        /// <returns>The <see cref="StreamPosition" />of the next event to process.</returns>
        Task<StreamPosition> FindNext(StreamId streamId, PartitionId partitionId, Microservice microservice, StreamPosition fromPosition, CancellationToken cancellationToken = default);
    }
}