﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 doLittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace doLittle.Queries.Coordination
{
    /// <summary>
    /// Defines a coordinator of queries
    /// </summary>
    public interface IQueryCoordinator
    {
        /// <summary>
        /// Execute a <see cref="IQuery"/>
        /// </summary>
        /// <param name="query"><see cref="IQuery"/> to execute</param>
        /// <param name="paging"><see cref="PagingInfo"/> applied to the query</param>
        /// <returns><see cref="QueryResult">Result</see> of the query</returns>
        QueryResult Execute(IQuery query, PagingInfo paging);
    }
}