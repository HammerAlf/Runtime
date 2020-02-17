// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Dolittle.Runtime.Events.Processing.Streams;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace Dolittle.Runtime.Events.Store.MongoDB.Processing
{
    /// <summary>
    /// Represents the state of an <see cref="StreamProcessor" />.
    /// </summary>
    public class StreamProcessorState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamProcessorState"/> class.
        /// </summary>
        /// <param name="id">The <see cref="StreamProcessorId" />.</param>
        /// <param name="position">The position.</param>
        /// <param name="failingPartitions">The states of the failing partitions.</param>
        public StreamProcessorState(StreamProcessorId id, uint position, IDictionary<Guid, FailingPartitionState> failingPartitions)
        {
            Id = id;
            Position = position;
            FailingPartitions = failingPartitions;
        }

        /// <summary>
        /// Gets or sets the <see cref="StreamProcessorId" />.
        /// </summary>
        [BsonId]
        public StreamProcessorId Id { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public uint Position { get; set; }

        /// <summary>
        /// Gets or sets the failing partitions.
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public IDictionary<Guid, FailingPartitionState> FailingPartitions { get; set; }

        /// <summary>
        /// Creates a new, initial, <see cref="Runtime.Events.Processing.Streams.StreamProcessorState" /> from a <see cref="Runtime.Events.Processing.EventProcessorId" />.
        /// </summary>
        /// <param name="id">The <see cref="StreamProcessorId" />.</param>
        /// <returns>The new initial <see cref="StreamProcessorState" />.</returns>
        public static StreamProcessorState NewFromId(Runtime.Events.Processing.Streams.StreamProcessorId id) => new StreamProcessorState(new StreamProcessorId(id.EventProcessorId, id.SourceStreamId), 0, new Dictionary<Guid, FailingPartitionState>());
    }
}