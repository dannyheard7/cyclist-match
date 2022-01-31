using Persistence.Entity;
using Persistence.Filter;
using Persistence.Repository;
using Persistence.Types.DTO;

namespace MatchingService;

internal class MatchingService : IMatchingService
{
    private const int MaximumDistanceKm = 10;
    private const double SpeedDeviationPercentage = 0.15;
    private const double DistanceDeviationPercentage = 0.3;

    private readonly IProfileRepository _profileRepository;

    public MatchingService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task MatchRelevantProfiles(Guid profiledId)
    {
        var profile = await _profileRepository.GetByUserId(profiledId);
        if (profile == null) throw new InvalidOperationException($"Could not find profile with id: {profiledId}");
        
        var averageSpeedDeviation =
            Convert.ToInt32(Math.Ceiling(profile.AverageSpeed * SpeedDeviationPercentage));
        var averageDistanceDeviation =
            Convert.ToInt32(Math.Ceiling(profile.AverageDistance * DistanceDeviationPercentage));

        var filter = new ProfileFilter(
            idFilter: new GuidFilter(new List<Guid> { profiledId }),
            locationFilter: new LocationFilter(profile.Location, MaximumDistanceKm),
            averageSpeedFilter: new IntegerFilter(profile.AverageSpeed-averageSpeedDeviation, profile.AverageSpeed+averageSpeedDeviation),
            averageDistanceFilter: new IntegerFilter(profile.AverageDistance-averageDistanceDeviation, profile.AverageDistance+averageDistanceDeviation),
            cyclingTypes: new CollectionFilter<CyclingType>(profile.CyclingTypes),
            availability: new CollectionFilter<Availability>(profile.Availability)
        );

        var results = await _profileRepository.Get(filter);
        throw new NotImplementedException();
    }
}