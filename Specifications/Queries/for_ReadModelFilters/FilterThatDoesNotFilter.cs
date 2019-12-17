﻿// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dolittle.ReadModels;

namespace Dolittle.Queries.Specs.for_ReadModelFilters
{
    public class FilterThatDoesNotFilter : ICanFilterReadModels
    {
        public bool FilterCalled = false;
        public IEnumerable<IReadModel> ReadModelsPassedForFilter = null;

        public IEnumerable<IReadModel> Filter(IEnumerable<IReadModel> readModels)
        {
            FilterCalled = true;
            ReadModelsPassedForFilter = readModels;
            return readModels;
        }
    }
}
