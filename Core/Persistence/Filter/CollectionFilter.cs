using System.Collections.Generic;

namespace Persistence.Filter;

public class CollectionFilter<T> where T : struct
{
    public CollectionFilter(IReadOnlyCollection<T> oneOf)
    {
        OneOf = oneOf;
    }
    
    public CollectionFilter(params T[] oneOf)
    {
        OneOf = oneOf;
    }

    public IReadOnlyCollection<T> OneOf { get; }
}