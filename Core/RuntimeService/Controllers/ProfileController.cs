using System;
using System.Threading.Tasks;
using Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Types.DTO;
using RuntimeService.Services;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly ICurrentUserService _currentUserService;
        
        public ProfileController(IProfileService profileService, ICurrentUserService currentUserService)
        {
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ProfileDTO>> Get(Guid userId)
        {
            var profile = await _profileService.GetById(userId);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        // [HttpPut("{userId}")]
        // public async Task<IActionResult> PutProfile(Guid userId, [FromBody] ProfileInputObject input)
        // {
        //     var profile = new Profile(userId, input.DisplayName, input.LocationName, input.Location, input.CyclingTypes,
        //         input.Availability, input.MinDistance, input.MaxDistance, input.Speed);
        //
        //     var updatedProfile = await _profileService.(profile);
        //     return Ok(updatedProfile);
        // }
    }
}