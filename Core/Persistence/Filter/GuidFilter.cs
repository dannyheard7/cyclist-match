using System;
using System.Collections;
using System.Collections.Generic;

namespace Persistence.Filter;

public class GuidFilter
{
    public IReadOnlyCollection<Guid>? NoneOf { get; private init; }
    
    public IReadOnlyCollection<Guid>? AnyOf { get; private init; }

    public static GuidFilter WithNoneOf(IReadOnlyCollection<Guid> ids) => new GuidFilter { NoneOf = ids };

    public static GuidFilter WithAnyOf(IReadOnlyCollection<Guid> ids) => new GuidFilter { AnyOf = ids };
}