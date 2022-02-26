using Persistence.Profile.Types.DTO;
using Persistence.SQL.Profile.Entites;

namespace Persistence.SQL.Profile.Mapper;

internal static class ProfileMapper
{
    public static ProfileDTO Map(this ProfileEntity profileEntity)
    {
        return new ProfileDTO(profileEntity.UserId,
            profileEntity.User.DisplayName,
            profileEntity.User.Picture,
            LocationMapper.Map(profileEntity.Location),
            profileEntity.CyclingTypes,
            profileEntity.Availability,
            profileEntity.AverageDistance,
            profileEntity.AverageSpeed,
            profileEntity.User.CreatedAt,
            profileEntity.User.UpdatedAt
        );
    }

    public static MatchDTO Map(this MatchEntity matchEntity)
    {
        return new MatchDTO(matchEntity.SourceUser.Profile.Map(), matchEntity.TargetUser.Profile.Map(), matchEntity.Relevance);
    }
}