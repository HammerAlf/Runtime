﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.Runtime.Events;
using Machine.Specifications;

namespace Dolittle.Runtime.Commands.Coordination.Specs.for_CommandContext.given
{
    public class a_command_context_for_a_simple_command_with_one_tracked_object : a_command_context_for_a_simple_command
    {
        protected static StatefulEventSource aggregated_root;
        protected static EventSourceId event_source_id;

        Establish context = () =>
        {
            event_source_id = Guid.NewGuid();
            aggregated_root = new StatefulEventSource(event_source_id);
            command_context.RegisterForTracking(aggregated_root);
        };
    }
}
