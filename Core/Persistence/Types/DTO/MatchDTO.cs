using System;

namespace Persistence.Types.DTO;

public class MatchDTO
{
    public MatchDTO(ProfileDTO sourceProfile, ProfileDTO targetProfile, float relevanceScore)
    {
        if(relevanceScore > 1 || relevanceScore < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(relevanceScore), "Must be between 0 and 1 (inclusive)");
        }
        
        SourceProfile = sourceProfile;
        TargetProfile = targetProfile;
        RelevanceScore = relevanceScore;
    }

    public ProfileDTO SourceProfile { get; }
    
    public ProfileDTO TargetProfile { get; }
    
    public float RelevanceScore { get; }
}