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

        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string LocationName { get; set; }
        public Location Location { get; set; }
        public ICollection<CyclingType> CyclingTypes { get; set; }
        public ICollection<AvailabilityItem> Availability { get; set; }
        public int MinDistance { get; set; }
        public int MaxDistance { get; set; }
        public int Speed { get; set;  }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}