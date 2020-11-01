using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth;
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
        private readonly ICurrentUserService _currentUserService;
        private readonly IProfileMatchService _profileMatchService;
        
        public ProfileController(IProfileService profileService, ICurrentUserService currentUserService, IProfileMatchService profileMatchService)
        {
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _profileMatchService = profileMatchService ?? throw new ArgumentNullException(nameof(profileMatchService));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var currentUser = await _currentUserService.GetUser();
            if(currentUser.Id != userId) throw new UnauthorizedAccessException();
            
            var profile = await _profileService.Get(userId);
            if (profile == null) return NotFound();
            return Ok(profile);
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
        
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutProfile(Guid userId, [FromBody] ProfileObject input)
        {
            var profile = new Profile(userId, input.DisplayName, input.LocationName, input.Location, input.CyclingTypes,
                input.Availability, input.MinDistance, input.MaxDistance, input.Speed);

            var updatedProfile = await _profileService.UpsertProfile(profile);
            return Ok(updatedProfile);
        }

        [HttpGet("matches")]
        public async Task<IActionResult> GetMatchingProfiles()
        {
            var currentUser = await _currentUserService.GetUser();
            return Ok(await _profileMatchService.GetProfileMatches(currentUser));
        }
    }
}