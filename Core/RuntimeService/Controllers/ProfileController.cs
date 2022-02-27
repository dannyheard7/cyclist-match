using System;
using System.Threading.Tasks;
using Hangfire;
using MatchingService;
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
        private readonly IMatchingService _matchingService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        
        public ProfileController(IProfileService profileService, IMatchingService matchingService, IUserContext userContext, IBackgroundJobClient backgroundJobClient)
        {
            _profileService = profileService;
            _matchingService = matchingService;
            _userContext = userContext;
            _backgroundJobClient = backgroundJobClient;
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

            _backgroundJobClient.Enqueue<IMatchingService>(service => service.MatchRelevantProfiles(profile.UserId));
            return Ok(profile);
        }
        
        // [HttpDelete("user")]
        // public async Task<ActionResult> DeleteUser()
        // {
        //     var currentUser = await _currentUserService.GetUser();
        //     await _userService.DeleteUser(currentUser);
        //
        //     return NoContent();
        // }
    }
}