using System;

namespace Persistence.Objects
{
    public class MatchingProfile
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
        public string LocationName { get; set; }
    }
}