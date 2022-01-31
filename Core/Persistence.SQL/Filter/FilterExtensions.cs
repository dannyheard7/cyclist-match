using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire.States;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Persistence.Filter;
using Persistence.SQL.Mapper;

namespace Persistence.SQL.Filters;

internal static class FilterExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> queryable, string propertyName, GuidFilter filter) where T : class
    {
        return queryable.Where(p => !filter.NoneOf.Contains(EF.Property<Guid>(p, propertyName)));
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
    
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> queryable, string propertyName, LocationFilter filter) where T : class
    {
        var convertedPoint = filter.Location.Map();
        return queryable.Where(p => EF.Property<Point>(p, propertyName).IsWithinDistance(convertedPoint, filter.MaxDistanceKm * 1000));
    }
}