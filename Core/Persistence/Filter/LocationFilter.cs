using System;
using Persistence.Types;

namespace Persistence.Filter;

public class LocationFilter
{
    public LocationFilter(Location location, int maxDistanceKm)
    {
        if (maxDistanceKm < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxDistanceKm), "Must be positive");
        }
        Location = location;
        MaxDistanceKm = maxDistanceKm;
    }

    public Location Location { get; }
    
    public int MaxDistanceKm { get; }
}