
using Persistence.Profile.Types.DTO;

namespace MatchingService;

internal class BasicRelevanceCalculator : IRelevanceCalculator
{
    public double Calculate(ProfileDTO a, ProfileDTO b)
    {
        // Let's start with distance
        var distance = a.Location.DistanceTo(b.Location);
        if (distance == 0) return 1;
        
        return 1 / a.Location.DistanceTo(b.Location);
    }
}