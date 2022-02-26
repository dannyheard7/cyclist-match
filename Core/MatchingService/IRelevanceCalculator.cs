using Persistence.Profile.Types.DTO;

namespace MatchingService;

internal interface IRelevanceCalculator
{
    public double Calculate(ProfileDTO profileA, ProfileDTO profileB);
}