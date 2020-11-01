using System.Collections.Generic;
using Persistence.Entity;

namespace RuntimeService.DTO
{
    public class ProfileMatch
    {
        public string DisplayName { get; }
        public string ProfileImage { get; }
        
        public string LocationName { get; }
        public int DistanceFromUserKM { get; }
        
        public ICollection<CyclingType> CyclingTypes { get; }
        public ICollection<AvailabilityItem> Availability { get; }
        public int MinDistance { get; }
        public int MaxDistance { get; }
        public int Speed { get; }
    }
}