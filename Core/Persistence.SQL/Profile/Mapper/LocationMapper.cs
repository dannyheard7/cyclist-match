using NetTopologySuite.Geometries;
using Location = Persistence.Profile.Types.Location;

namespace Persistence.SQL.Profile.Mapper;

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