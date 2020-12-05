using System;
using System.Collections.Generic;
using Persistence.Entity;

namespace RuntimeService.DTO
{
    public class ProfileMatch
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImage { get; }
        
        public string LocationName { get; set; }
        public int DistanceFromUserKM { get; }
        
        public ICollection<CyclingType> CyclingTypes { get; }
        public ICollection<AvailabilityItem> Availability { get; }
        public int MinDistance { get; }
        public int MaxDistance { get; }
        public int Speed { get; }
    }
}