using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RuntimeService.Services;
using System.Threading.Tasks;
using System;
using Auth;

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
    }
}