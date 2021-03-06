// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;

namespace Dolittle.Runtime.Events.for_EventSourceVersion.when_incrementing_the_sequence
{
    [Subject(typeof(EventSourceVersion), "NextSequence")]
    public class on_a_version
    {
        static EventSourceVersion current;
        static EventSourceVersion result;

        Establish context = () => current = new EventSourceVersion(3, 0);

        Because of = () => result = current.NextSequence();

        It should_be_the_same_commit = () => result.Commit.ShouldEqual(current.Commit);
        It should_be_the_next_sequence = () => result.Sequence.ShouldEqual(current.Sequence + 1);
    }
}