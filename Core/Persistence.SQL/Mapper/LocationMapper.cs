using NetTopologySuite.Geometries;
using Persistence.Filter;
using Location = Persistence.Types.Location;

namespace Persistence.SQL.Mapper;

internal static class LocationMapper
{
    public static Point Map(this Location location, int srid = 4326)
    {
        return new Point(location.Longitude, location.Longitude) { SRID = srid };
    }

    public static Location Map(this Point location)
    {
        return new Location(location.X, location.Y);
    }
}