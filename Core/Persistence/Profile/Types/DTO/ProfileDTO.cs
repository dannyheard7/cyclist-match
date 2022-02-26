using System;
using System.Collections.Generic;
using Persistence.Profile;

namespace Persistence.Profile.Types.DTO
{
    public class ProfileDTO
    {
        public ProfileDTO(
            Guid userId,
            string userDisplayName,
            string? userPicture,
            Location location,
            IReadOnlyCollection<CyclingType> cyclingTypes,
            IReadOnlyCollection<Availability> availability,
            int averageDistance,
            int averageSpeed,
            DateTime createdAt,
            DateTime updatedAt)
        {
            UserId = userId;
            UserDisplayName = userDisplayName;
            UserPicture = userPicture;
            Location = location;
            CyclingTypes = cyclingTypes;
            Availability = availability;
            AverageDistance = averageDistance;
            AverageSpeed = averageSpeed;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public Guid UserId { get; }
        public string UserDisplayName { get; }
        public string? UserPicture { get; }
        public Location Location { get; }
        public IReadOnlyCollection<CyclingType> CyclingTypes { get; }
        public IReadOnlyCollection<Availability> Availability { get; }
        public int AverageDistance { get; }
        public int AverageSpeed { get; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }
    }
}