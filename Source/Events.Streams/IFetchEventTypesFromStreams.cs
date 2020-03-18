// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Applications;
using Dolittle.Artifacts;

namespace Dolittle.Runtime.Events.Streams
{
    /// <summary>
    /// Defines a system that can fetch <see cref="ArtifactId">event types</see> from <see cref="StreamId">streams</see>.
    /// </summary>
    public interface IFetchEventTypesFromStreams
    {
        /// <summary>
        /// Fetch the unique <see cref="Artifact">event types</see> in an inclusive range in a <see cref="StreamId" />.
        /// </summary>
        /// <param name="streamId"><see cref="StreamId">the stream in the event store</see>.</param>
        /// <param name="microservice">The source <see cref="Microservice" />.</param>
        /// <param name="range">The <see cref="StreamPositionRange" />.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
        /// <returns>The <see cref="IEnumerable{Artifact}" /> event types.</returns>
        Task<IEnumerable<Artifact>> FetchTypesInRange(StreamId streamId, Microservice microservice, StreamPositionRange range, CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetch the unique <see cref="Artifact">event types</see> in a an inclusive range in a <see cref="StreamId" /> and <see cref="PartitionId" />.
        /// </summary>
        /// <param name="streamId"><see cref="StreamId">the stream in the event store</see>.</param>
        /// <param name="partitionId">The <see cref="PartitionId" />.</param>
        /// <param name="microservice">The source <see cref="Microservice" />.</param>
        /// <param name="range">The <see cref="StreamPositionRange" />.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" />.</param>
        /// <returns>The <see cref="IEnumerable{Artifact}" /> event types.</returns>
        Task<IEnumerable<Artifact>> FetchTypesInRangeAndPartition(StreamId streamId, PartitionId partitionId, Microservice microservice, StreamPositionRange range, CancellationToken cancellationToken = default);
    }
}