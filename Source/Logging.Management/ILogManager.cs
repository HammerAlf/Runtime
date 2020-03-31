// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
#if false
using System.Collections.ObjectModel;
using Dolittle.Logging.Json;

namespace Dolittle.Runtime.Logging.Management
{
    /// <summary>
    /// Defines a system for managing logs for management purposes.
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Gets a <see cref="ObservableCollection{T}"/> of <see cref="JsonLogMessage">log messages</see>.
        /// </summary>
        ObservableCollection<JsonLogMessage> Messages { get; }

        /// <summary>
        /// Write a log message.
        /// </summary>
        /// <param name="message"><see cref="JsonLogMessage"/> to write.</param>
        void Write(JsonLogMessage message);
    }
}
#endif