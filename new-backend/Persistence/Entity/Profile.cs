using System;

namespace Persistence.Entity
{
    public class Profile
    {
        public string UserId { get; set; }
        
        public string LocationName { get; set; }
        
        public Location Location { get; set; }
        
        public CyclingType[] CyclingTypes { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}