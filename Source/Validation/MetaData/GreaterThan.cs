﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Validation.MetaData
{
    /// <summary>
    /// Represents the metadata for the GreaterThan validation rule.
    /// </summary>
    public class GreaterThan : Rule
    {
        /// <summary>
        /// Gets or sets the value that values validated up against must be greater than.
        /// </summary>
        public object Value { get; set; }
    }
}
