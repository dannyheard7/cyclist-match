using System.Collections.Generic;
using Persistence.Profile.Types;

namespace RuntimeService.Controllers.Models;

public class ProfileInput
{
    public ProfileInput(string displayName, int averageDistance, int averageSpeed, Location location, IReadOnlyCollection<Availability> availability, IReadOnlyCollection<CyclingType> cyclingTypes)
    {
        DisplayName = displayName;
        AverageDistance = averageDistance;
        AverageSpeed = averageSpeed;
        Location = location;
        Availability = availability;
        CyclingTypes = cyclingTypes;
    }

    public string DisplayName { get; }
    public int AverageDistance { get; }
    public int AverageSpeed { get;  }
    public Location Location { get; }
    public IReadOnlyCollection<Availability> Availability { get; }
    public IReadOnlyCollection<CyclingType> CyclingTypes { get; }
}