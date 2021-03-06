﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dolittle.Events;
using Dolittle.Execution;
using Dolittle.Runtime.Transactions;

namespace Dolittle.Runtime.Commands.Coordination
{
    /// <summary>
    /// Defines a context for a <see cref="CommandRequest">command</see> passing through the system.
    /// </summary>
    public interface ICommandContext : ITransaction
    {
        /// <summary>
        /// Gets the <see cref="CorrelationId"/> for the <see cref="ICommandContext"/>.
        /// </summary>
        CorrelationId CorrelationId { get; }

        /// <summary>
        /// Gets the <see cref="CommandRequest">command</see> the context is for.
        /// </summary>
        CommandRequest Command { get; }

        /// <summary>
        /// Gets the <see cref="ExecutionContext"/> for the command.
        /// </summary>
        ExecutionContext ExecutionContext { get; }

        /// <summary>
        /// Register an <see cref="IEventSource">event source</see> for tracking.
        /// </summary>
        /// <param name="eventSource"><see cref="IEventSource"/> being tracked.</param>
        void RegisterForTracking(IEventSource eventSource);

        /// <summary>
        /// Get objects that are being tracked.
        /// </summary>
        /// <returns>All tracked objects.</returns>
        IEnumerable<IEventSource> GetObjectsBeingTracked();
    }
}