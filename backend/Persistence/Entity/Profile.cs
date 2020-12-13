using System;
using System.Collections.Generic;

namespace Persistence.Entity
{
    public class Profile
    {
        protected Profile()
        {
        }

        public Profile(Guid userId, string displayName, string locationName, Location location,
            ICollection<CyclingType> cyclingTypes, ICollection<AvailabilityItem> availability, int minDistance, int maxDistance, int speed)
        {
            UserId = userId;
            DisplayName = displayName;
            LocationName = locationName;
            Location = location;
            CyclingTypes = cyclingTypes;
            Availability = availability;
            MinDistance = minDistance;
            MaxDistance = maxDistance;
            Speed = speed;
        }

        public Guid UserId { get; }
        public string DisplayName { get; }
        public string LocationName { get; }
        public Location Location { get; }
        public ICollection<CyclingType> CyclingTypes { get; }
        public ICollection<AvailabilityItem> Availability { get; }
        public int MinDistance { get; }
        public int MaxDistance { get; }
        public int Speed { get; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}