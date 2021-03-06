// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Dolittle.Runtime.Events.Relativity
{
    /// <summary>
    /// Defines the point of no return for events.
    /// </summary>
    /// <remarks>
    /// The event horizon represents the final entry for committed events.
    /// At this point they can only be seen by other singularities.
    /// </remarks>
    public interface IEventHorizon
    {
        /// <summary>
        /// Gets the collection of <see cref="ISingularity">singularities</see> in the system.
        /// </summary>
        IEnumerable<ISingularity> Singularities { get; }

        /// <summary>
        /// Pass events through the <see cref="IEventHorizon"/>.
        /// </summary>
        /// <param name="committedEventStream"><see cref="CommittedEvents"/> to pass through.</param>
        void PassThrough(Dolittle.Runtime.Events.Processing.CommittedEventStreamWithContext committedEventStream);

        /// <summary>
        /// Gravitate towards <see cref="ISingularity"/>.
        /// </summary>
        /// <param name="singularity"><see cref="ISingularity"/> that will get gravitated towards.</param>
        /// <param name="tenantOffsets">The offsets of processed commits from another bounded context.</param>
        void GravitateTowards(ISingularity singularity, IEnumerable<TenantOffset> tenantOffsets);

        /// <summary>
        /// When a singularity collapses, this method is called to let the <see cref="IEventHorizon"/> know.
        /// </summary>
        /// <param name="singularity"><see cref="ISingularity"/> that collapsed.</param>
        void Collapse(ISingularity singularity);
    }
}