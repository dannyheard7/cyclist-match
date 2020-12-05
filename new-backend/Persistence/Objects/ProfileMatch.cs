using System;
using System.Collections.Generic;
using Persistence.Entity;

namespace RuntimeService.DTO
{
    public class ProfileMatch
    {
        public ProfileMatch(
            Guid userId,
            string displayName,
            string locationName,
            decimal distanceFromUserKm,
            ICollection<CyclingType> cyclingTypes,
            ICollection<AvailabilityItem> availability,
            int minDistance,
            int maxDistance,
            int speed,
            string? profileImage=null)
        {
            UserId = userId;
            DisplayName = displayName;
            ProfileImage = profileImage;
            LocationName = locationName;
            DistanceFromUserKM = distanceFromUserKm;
            CyclingTypes = cyclingTypes;
            Availability = availability;
            MinDistance = minDistance;
            MaxDistance = maxDistance;
            Speed = speed;
        }

        public Guid UserId { get;  }
        public string DisplayName { get; }
        public string? ProfileImage { get; }
        
        public string LocationName { get; }
        public decimal DistanceFromUserKM { get; }
        
        public ICollection<CyclingType> CyclingTypes { get; }
        public ICollection<AvailabilityItem> Availability { get; }
        public int MinDistance { get; }
        public int MaxDistance { get; }
        public int Speed { get; }
    }
}