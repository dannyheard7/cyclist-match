using System;
using System.Collections.Generic;
using Persistence.Entity;

namespace Persistence.Types.DTO;

public class CreateProfileDTO : ProfileDTO
{
    public CreateProfileDTO(Guid userId, string userDisplayName, string userPicture, Location location, IReadOnlyCollection<CyclingType> cyclingTypes, IReadOnlyCollection<Availability> availability, int averageDistance, int averageSpeed, DateTime createdAt, DateTime updatedAt, string externalUserId) : base(userId, userDisplayName, userPicture, location, cyclingTypes, availability, averageDistance, averageSpeed, createdAt, updatedAt)
    {
        ExternalUserId = externalUserId;
    }

    public string ExternalUserId { get; }
}