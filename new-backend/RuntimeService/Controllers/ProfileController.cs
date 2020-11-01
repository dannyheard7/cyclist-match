using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Entity;
using RuntimeService.Services;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("profiles")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var profile = await _profileService.Get(userId);

            if (profile == null) return NotFound();
            return Ok(profile);
        }
        
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutProfile(Guid userId, [FromBody] ProfileObject input)
        {
            var profile = new Profile(userId, input.DisplayName, input.LocationName, input.Location, input.CyclingTypes,
                input.Availability, input.MinDistance, input.MaxDistance, input.Speed);

            var updatedProfile = await _profileService.UpsertProfile(profile);
            return Ok(updatedProfile);
        }

        public class ProfileObject
        {
            public string DisplayName { get; set; }
            public string LocationName { get; set; }
            public int MinDistance { get; set; }
            public int MaxDistance { get; set; }
            public int Speed { get; set;  }
            public Location Location { get; set; }
            public ICollection<AvailabilityItem> Availability { get; set; }
            public ICollection<CyclingType> CyclingTypes { get; set; }
        }
    }
}