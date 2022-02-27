using System;
using System.Collections.Generic;
using System.Linq;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Persistence.Filter;

namespace Persistence.SQL.Filters;

internal static class FilterExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> queryable, string propertyName, GuidFilter filter) where T : class
    {
        if (filter.NoneOf != null)
        {
            return queryable.Where(p => !filter.NoneOf.Contains(EF.Property<Guid>(p, propertyName)));
        }
        
        if(filter.AnyOf != null)
        {
            return queryable.Where(p => filter.AnyOf.Contains(EF.Property<Guid>(p, propertyName)));
        }

        throw new InvalidFilterException();
    }

    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> queryable, string propertyName, IntegerFilter filter) where T : class
    {
        if (filter.EqualTo != null)
        {
            return queryable.Where(x => EF.Property<int>(x, propertyName) == filter.EqualTo);
        }
        
        if(filter.GreaterThan != null && filter.LessThan != null)
        {
            return queryable.Where(x => EF.Property<int>(x, propertyName) > filter.GreaterThan && EF.Property<int>(x, propertyName) < filter.LessThan);
        }

        throw new InvalidFilterException();
    }

    public static IQueryable<T> ApplyFilter<T, TV>(this IQueryable<T> queryable, string propertyName, CollectionFilter<TV> filter) where TV : struct
    {
        var predicate = PredicateBuilder.New<T>();
        
        foreach(var oneOf in filter.OneOf)
        {
            predicate = predicate.Or (p => EF.Property<ICollection<TV>>(p, propertyName).Contains(oneOf));
        }

        return queryable.Where(predicate);
    }
}