using System.Collections.Generic;
using Persistence.Entity;

namespace Persistence.Filter;

public class ProfileFilter
{
    public ProfileFilter(
        GuidFilter? idFilter = null,
        string? searchTerm = null,
        LocationFilter? locationFilter = null,
        CollectionFilter<CyclingType>? cyclingTypes = null,
        CollectionFilter<Availability>? availability = null,
        IntegerFilter? averageDistanceFilter = null,
        IntegerFilter? averageSpeedFilter = null)
    {
        IdFilter = idFilter;
        SearchTerm = searchTerm;
        LocationFilter = locationFilter;
        CyclingTypes = cyclingTypes;
        Availability = availability;
        AverageDistanceFilter = averageDistanceFilter;
        AverageSpeedFilter = averageSpeedFilter;
    }
    
    public GuidFilter? IdFilter { get; }

    public string? SearchTerm { get; }

    public LocationFilter? LocationFilter { get; }

    public CollectionFilter<CyclingType>? CyclingTypes { get; }

    public CollectionFilter<Availability>? Availability { get; }

    public IntegerFilter? AverageDistanceFilter { get; }
    
    public IntegerFilter? AverageSpeedFilter { get; }
}