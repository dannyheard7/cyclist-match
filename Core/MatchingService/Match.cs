using Persistence.Types.DTO;

namespace MatchingService;

public class Match
{
    public Match(ProfileDTO profileA, ProfileDTO profileB, decimal relevanceScore)
    {
        ProfileA = profileA;
        ProfileB = profileB;
        RelevanceScore = relevanceScore;
    }

    public ProfileDTO ProfileA { get; }
    
    public ProfileDTO ProfileB { get; }
    
    public decimal RelevanceScore { get; }
}