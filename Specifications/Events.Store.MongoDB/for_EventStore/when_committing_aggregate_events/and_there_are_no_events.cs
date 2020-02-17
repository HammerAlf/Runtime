// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.Logging;
using Machine.Specifications;

namespace Dolittle.Runtime.Events.Store.MongoDB.for_EventStore.when_committing_aggregate_events
{
    public class and_there_are_no_events : given.all_dependencies
    {
        static EventStore event_store;
        static UncommittedAggregateEvents uncommitted_events;
        static EventSourceId event_source;
        static Artifacts.Artifact aggregate_root;
        static AggregateRootVersion aggregate_root_version;
        static Exception exception;

        Establish context = () =>
        {
            event_source = Guid.NewGuid();
            aggregate_root = new Artifacts.Artifact(Guid.NewGuid(), 0);
            aggregate_root_version = AggregateRootVersion.Initial;
            uncommitted_events = new UncommittedAggregateEvents(event_source, aggregate_root, aggregate_root_version, Array.Empty<UncommittedEvent>());
            event_store = new EventStore(
                execution_context_manager.Object,
                an_event_store_connection,
                event_committer,
                aggregate_roots,
                Moq.Mock.Of<ILogger>());
        };

        Because of = () => exception = Catch.Exception(() => event_store.CommitAggregateEvents(uncommitted_events).GetAwaiter().GetResult());

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
        It should_fail_because_there_are_no_events_to_commit = () => exception.ShouldBeOfExactType<NoEventsToCommit>();
    }
}