﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;

namespace Dolittle.Queries.Specs.for_QueryResult
{
    public class when_exception_is_null_and_it_items_is_null
    {
        static QueryResult result;

        Establish context = () => result = new QueryResult();

        Because of = () =>
        {
            result.Exception = null;
            result.Items = null;
        };

        It should_not_be_successful = () => result.Success.ShouldBeFalse();
    }
}
