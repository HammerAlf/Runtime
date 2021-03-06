﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using Dolittle.Events;

namespace Dolittle.Runtime.Events
{
    /// <summary>
    /// Represents a special version of an eventstream
    /// that holds committed <see cref="IEvent">events</see>.
    /// </summary>
    public class CommittedEvents : IEnumerable<CommittedEvent>
    {
        readonly List<CommittedEvent> _events = new List<CommittedEvent>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommittedEvents"/> class.
        /// </summary>
        /// <param name="eventSourceId">The <see cref="EventSourceId"/> of the <see cref="IEventSource"/>.</param>
        public CommittedEvents(EventSourceId eventSourceId)
        {
            EventSourceId = eventSourceId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommittedEvents"/> class.
        /// </summary>
        /// <param name="eventSourceId">The <see cref="EventSourceId"/> of the <see cref="IEventSource"/>.</param>
        /// <param name="committedEvents">The <see cref="CommittedEvent">events</see>.</param>
        public CommittedEvents(EventSourceId eventSourceId, IEnumerable<CommittedEvent> committedEvents)
        {
            EventSourceId = eventSourceId;
            foreach (var committedEvent in committedEvents)
            {
                EnsureEventIsValid(committedEvent);
                _events.Add(committedEvent);
            }
        }

        /// <summary>
        /// Gets the Id of the <see cref="IEventSource"/> that this <see cref="CommittedEvents"/> relates to.
        /// </summary>
        public EventSourceId EventSourceId { get; }

        /// <summary>
        /// Gets a value indicating whether there are any events in the Stream or not.
        /// </summary>
        public bool HasEvents => Count > 0;

        /// <summary>
        /// Gets the number of Events in the Stream.
        /// </summary>
        public int Count => _events.Count;

        /// <summary>
        /// Get a generic enumerator to iterate over the events.
        /// </summary>
        /// <returns><see cref="IEnumerator{T}"/> of <see cref="CommittedEvent"/>.</returns>
        public IEnumerator<CommittedEvent> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void EnsureEventIsValid(CommittedEvent committedEvent)
        {
            if (committedEvent.Event == null)
            {
                throw new EventCanNotBeNull();
            }

            if (committedEvent.Metadata.EventSourceId != EventSourceId)
                throw new EventBelongsToOtherEventSource(committedEvent.Metadata?.EventSourceId ?? Guid.NewGuid(), EventSourceId);
        }
    }
}