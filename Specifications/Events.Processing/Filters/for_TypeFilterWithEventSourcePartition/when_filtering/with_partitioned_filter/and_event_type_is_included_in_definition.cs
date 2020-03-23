// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Dolittle.Artifacts;
using Dolittle.Logging;
using Dolittle.Runtime.Events.Streams;
using Machine.Specifications;

namespace Dolittle.Runtime.Events.Processing.Filters.for_TypeFilterWithEventSourcePartition.when_filtering.with_partitioned_filter
{
    public class and_event_type_is_included_in_definition : given.all_dependencies
    {
        static Artifact artifact;
        static PartitionId partition;
        static TypeFilterWithEventSourcePartition filter;
        static IFilterResult result;

        Establish context = () =>
        {
            artifact = given.artifacts.single();
            partition = Guid.NewGuid();
            filter = new TypeFilterWithEventSourcePartition(
                new TypeFilterWithEventSourcePartitionDefinition(Guid.NewGuid(), Guid.NewGuid(), new ArtifactId[] { artifact.Id }.AsEnumerable(), true),
                writer.Object,
                Moq.Mock.Of<ILogger>());
        };

        Because of = () => result = filter.Filter(given.committed_events.single_with_artifact(partition.Value, artifact), Guid.NewGuid(), Guid.NewGuid(), default).GetAwaiter().GetResult();

        It should_have_the_correct_partition = () => result.Partition.ShouldEqual(partition);
        It should_be_successful = () => result.Succeeded.ShouldBeTrue();
        It should_not_retry = () => result.Retry.ShouldBeFalse();
        It should_be_included = () => result.IsIncluded.ShouldBeTrue();
    }
}