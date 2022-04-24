using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Profile.Types.DTO;
using ProfileService;
using RuntimeService.Controllers.Models;
using RuntimeService.Services;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IUserContext _userContext;

        public ProfileController(IProfileService profileService, IUserContext userContext)
        {
            _profileService = profileService;
            _userContext = userContext;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<ProfileDTO>> Get(Guid userId)
        {
            var profile = await _profileService.GetById(userId);
            if (profile == null) return NotFound();

            return Ok(profile);
        }

        [HttpPost]
        public async Task<ActionResult<ProfileDTO>> CreateProfile([FromBody] ProfileInput input)
        {
            var profile = await _userContext.CreateProfileForUser(input);
            return Ok(profile);
        }
        
        [HttpDelete("user")]
        public async Task<ActionResult> DeleteUser()
        {
            var currentUser = await _userContext.GetUserProfile();
            await _profileService.Delete(currentUser);
        
            return NoContent();
        }
    }
}