// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

extern alias contracts;

using contracts::Dolittle.Runtime.Events.Processing;

namespace Dolittle.Runtime.Events.Processing.EventHorizon
{
    /// <summary>
    /// Extension methods for <see cref="ExternalEventHandlerClientToRuntimeResponse" />.
    /// </summary>
    public static class ExternalEventHandlerClientToRuntimeResponseExtensions
    {
        /// <summary>
        /// Converts the <see cref="ExternalEventHandlerClientToRuntimeResponse" /> to a <see cref="IProcessingResult" />.
        /// </summary>
        /// <param name="response">The <see cref="ExternalEventHandlerClientToRuntimeResponse" />.</param>
        /// <returns>The converted <see cref="IProcessingResult" />.</returns>
        public static IProcessingResult ToProcessingResult(this ExternalEventHandlerClientToRuntimeResponse response)
        {
            if (!response.Succeeded && !response.Retry) return new FailedProcessingResult(response.FailureReason ?? string.Empty);
            else if (!response.Succeeded && response.Retry) return new RetryProcessingResult(response.RetryTimeout, response.FailureReason ?? string.Empty);
            return new SucceededProcessingResult();
        }
    }
}