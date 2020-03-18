// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.Applications;

namespace Dolittle.Runtime.Events.Processing.EventHorizon
{
    /// <summary>
    /// Exception that gets thrown when the microservice value of an external events handler is invalid.
    /// </summary>
    public class InvalidMicroserviceForExternalEvents : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidMicroserviceForExternalEvents"/> class.
        /// </summary>
        /// <param name="microservice">The <see cref="Microservice" />.</param>
        public InvalidMicroserviceForExternalEvents(Microservice microservice)
            : base($"Microservice for external events cannot be '{microservice}")
        {
        }
    }
}