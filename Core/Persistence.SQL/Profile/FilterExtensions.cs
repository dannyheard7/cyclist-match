using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Persistence.Profile.Filter;
using Persistence.SQL.Profile.Mapper;

namespace Persistence.SQL.Profile;

public static class FilterExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> queryable, string propertyName, LocationFilter filter) where T : class
    {
        var convertedPoint = filter.Location.Map();
        return queryable.Where(p => EF.Property<Point>(p, propertyName).IsWithinDistance(convertedPoint, filter.MaxDistanceKm * 1000));
    }
}