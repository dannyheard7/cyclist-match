using System;
using System.Collections;
using System.Collections.Generic;

namespace Persistence.Filter;

public class GuidFilter
{
    public GuidFilter(IReadOnlyCollection<Guid>? noneOf)
    {
        NoneOf = noneOf ?? new List<Guid>();
    }

    public IReadOnlyCollection<Guid> NoneOf { get; }
}