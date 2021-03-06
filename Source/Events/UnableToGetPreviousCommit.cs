// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Runtime.Events
{
    /// <summary>
    /// Exception that gets thrown when an <see cref="EventSourceVersion" /> is invalid and one can't get the next sequence in the commit.
    /// </summary>
    public class UnableToGetPreviousCommit : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnableToGetPreviousCommit"/> class.
        /// </summary>
        /// <param name="version">A message describing the exception.</param>
        public UnableToGetPreviousCommit(EventSourceVersion version)
            : base($"Cannot get the Previous Commit of Commit {version.Commit}")
        {
        }
    }
}