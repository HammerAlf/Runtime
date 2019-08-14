/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Grpc.Core;

namespace Dolittle.Runtime.Server
{
    /// <summary>
    /// Defines a system that can bind a gRPC service for management purpose
    /// </summary>
    public interface ICanBindManagementServices : ICanBindServices
    {
    }
}