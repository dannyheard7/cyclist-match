using System.Collections.Generic;
using Persistence.Entity;
using Persistence.SQL.Entities;
using Persistence.Types;
using Persistence.Types.DTO;

namespace Persistence.SQL.Mapper;

internal static class ProfileMapper
{
    public static ProfileDTO Map(this ProfileEntity profileEntity)
    {
        return new ProfileDTO(profileEntity.UserId,
            profileEntity.User.DisplayName,
            profileEntity.User.Picture,
            new Location(profileEntity.Location.X, profileEntity.Location.Y),
            profileEntity.CyclingTypes,
            profileEntity.Availability,
            profileEntity.AverageDistance,
            profileEntity.AverageSpeed,
            profileEntity.User.CreatedAt,
            profileEntity.User.UpdatedAt
        );
    }
}