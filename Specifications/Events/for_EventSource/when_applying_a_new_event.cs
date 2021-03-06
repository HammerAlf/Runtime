﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Runtime.Events;
using Machine.Specifications;

namespace Dolittle.Events.Specs.for_EventSource
{
    [Subject(Subjects.applying_events)]
    public class when_applying_a_new_event : given.a_stateful_event_source
    {
        Establish context = () => @event = new SimpleEvent();

        Because of = () => event_source.Apply(@event);

        It should_add_the_event_to_the_uncommited_events = () => event_source.UncommittedEvents.ShouldContainOnly(@event);
        It should_increment_the_sequence_of_the_version = () => event_source.Version.Sequence.ShouldEqual(1u);
        It should_not_increment_the_commit_of_the_version = () => event_source.Version.Commit.ShouldEqual(EventSourceVersion.Initial.Commit);
        It should_call_the_on_method_for_the_event = () => event_source.EventApplied.ShouldBeTrue();
    }
}