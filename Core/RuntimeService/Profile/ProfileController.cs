using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth;
using Hangfire;
using MatchingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Types.DTO;
using RuntimeService.ProfileApi;
using RuntimeService.Services;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("api/profiles")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IMatchingService _matchingService;
        private readonly IUserContext _userContext;
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
            var currentUser = await _userContext.GetUser();
           
            var profile = new CreateProfileDTO(
                Guid.NewGuid(),
                input.DisplayName,
                currentUser.Picture,
                input.Location,
                input.CyclingTypes,
                input.Availability,
                input.AverageDistance,
                input.AverageSpeed,
                DateTime.UtcNow, 
                DateTime.UtcNow,
                currentUser.Id);
        
            await _profileService.Create(profile);

            _backgroundJobClient.Enqueue<IMatchingService>(service => service.MatchRelevantProfiles(profile.UserId));
            return Ok(profile);
        }

        public class MatchesResponse
        {
            public MatchesResponse(IEnumerable<ProfileDTO> matches)
            {
                Matches = matches;
            }

            public IEnumerable<ProfileDTO> Matches { get; }
        }
        
        [HttpGet("{userId}/matches")]
        public async Task<ActionResult<MatchesResponse>> GetProfileMatches(Guid userId)
        {
            var profile = await _profileService.GetById(userId);
            if (profile == null) return NotFound();

            var matches = await _matchingService.GetMatchedProfiles(profile);
            return Ok(new MatchesResponse(matches));
        }
    }
}