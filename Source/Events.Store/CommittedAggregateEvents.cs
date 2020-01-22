// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using Dolittle.Artifacts;
using Dolittle.Collections;
using Dolittle.Events;

namespace Dolittle.Runtime.Events.Store
{
    /// <summary>
    /// Represents a sequence of <see cref="IEvent"/>s applied by an AggregateRoot to an Event Source that have been committed to the Event Store.
    /// </summary>
    public class CommittedAggregateEvents : IReadOnlyList<CommittedAggregateEvent>
    {
        readonly NullFreeList<CommittedAggregateEvent> _events;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommittedAggregateEvents"/> class.
        /// </summary>
        /// <param name="eventSource">The <see cref="EventSourceId"/> that the Events were applied to.</param>
        /// <param name="aggregateRoot">The <see cref="Artifact"/> representing the type of the Aggregate Root that applied the Event to the Event Source.</param>
        /// <param name="aggregateRootVersion">The version of the <see cref="AggregateRoot"/> that applied the Events.</param>
        /// <param name="events">The <see cref="CommittedAggregateEvent">events</see>.</param>
        public CommittedAggregateEvents(EventSourceId eventSource, Artifact aggregateRoot, AggregateRootVersion aggregateRootVersion, IReadOnlyList<CommittedAggregateEvent> events)
        {
            EventSource = eventSource;
            AggregateRoot = aggregateRoot;
            AggregateRootVersion = aggregateRootVersion;

            for (var i = 0; i < events.Count; i++)
            {
                var @event = events[i];
                ThrowIfEventIsNull(@event);
                ThrowIfEventWasAppliedToOtherEventSource(@event);
                ThrowIfEventWasAppliedByOtherAggregateRoot(@event);
                ThrowIfAggreggateRootVersionIsOutOfOrder(@event, AggregateRootVersion + (uint)i);
                if (i > 0) ThrowIfEventLogVersionIsOutOfOrder(@event, events[i - 1]);
            }

            _events = new NullFreeList<CommittedAggregateEvent>(events);
        }

        /// <summary>
        /// Gets the Event Source that the Events were applied to.
        /// </summary>
        public EventSourceId EventSource { get; }

        /// <summary>
        /// Gets the <see cref="Artifact"/> representing the type of the Aggregate Root that applied the Event to the Event Source.
        /// </summary>
        public Artifact AggregateRoot { get; }

        /// <summary>
        /// Gets the version of the <see cref="AggregateRoot"/> that applied the Events.
        /// </summary>
        public AggregateRootVersion AggregateRootVersion { get; }

        /// <summary>
        /// Gets a value indicating whether or not there are any events in the committed sequence.
        /// </summary>
        public bool HasEvents => Count > 0;

        /// <inheritdoc/>
        public int Count => _events.Count;

        /// <inheritdoc/>
        public CommittedAggregateEvent this[int index] => _events[index];

        /// <inheritdoc/>
        public IEnumerator<CommittedAggregateEvent> GetEnumerator() => _events.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => _events.GetEnumerator();

        void ThrowIfEventIsNull(CommittedAggregateEvent @event)
        {
            if (@event == null) throw new EventCanNotBeNull();
        }

        void ThrowIfEventWasAppliedToOtherEventSource(CommittedAggregateEvent @event)
        {
            if (@event.EventSource != EventSource) throw new EventWasAppliedToOtherEventSource(@event.EventSource, EventSource);
        }

        void ThrowIfEventWasAppliedByOtherAggregateRoot(CommittedAggregateEvent @event)
        {
            if (@event.AggregateRoot != AggregateRoot) throw new EventWasAppliedByOtherAggregateRoot(@event.AggregateRoot, AggregateRoot);
        }

        void ThrowIfAggreggateRootVersionIsOutOfOrder(CommittedAggregateEvent @event, AggregateRootVersion expectedVersion)
        {
            if (@event.AggregateRootVersion != expectedVersion) throw new AggregateRootVersionIsOutOfOrder(@event.AggregateRootVersion, expectedVersion);
        }

        void ThrowIfEventLogVersionIsOutOfOrder(CommittedAggregateEvent @event, CommittedAggregateEvent previousEvent)
        {
            if (@event.EventLogVersion != previousEvent.EventLogVersion + 1) throw new EventLogVersionIsOutOfOrder(@event.EventLogVersion, previousEvent.EventLogVersion + 1);
        }
    }
}