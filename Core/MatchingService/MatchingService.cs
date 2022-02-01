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
    private readonly IProfileMatchRepository _profileMatchRepository;
    private readonly IRelevanceCalculator _relevanceCalculator;
    
    public MatchingService(IProfileRepository profileRepository, IProfileMatchRepository profileMatchRepository, IRelevanceCalculator relevanceCalculator)
    {
        _profileRepository = profileRepository;
        _profileMatchRepository = profileMatchRepository;
        _relevanceCalculator = relevanceCalculator;
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

        var matches = results
            .Select(x =>
            {
                var exactRelevance = _relevanceCalculator.Calculate(profile, x);
                var normalisedRelevance = (float)Math.Round(Math.Max(Math.Min(exactRelevance, 0), 1), 5);
                
                return new MatchDTO(profile, x, normalisedRelevance);
            })
            .ToList();

        await _profileMatchRepository.UpdateProfileMatches(profile, matches);
    }

    public Task<IEnumerable<ProfileDTO>> GetMatchedProfiles(ProfileDTO profile)
    {
        return _profileMatchRepository.GetMatchedProfiles(profile);
    }
}