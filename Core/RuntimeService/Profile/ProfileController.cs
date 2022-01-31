﻿using System;
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
        private readonly ICurrentUserService _currentUserService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        
        public ProfileController(
            IProfileService profileService,
            ICurrentUserService currentUserService,
            IBackgroundJobClient backgroundJobClient)
        {
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _backgroundJobClient = backgroundJobClient ?? throw new ArgumentNullException(nameof(backgroundJobClient));
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
            var currentUser = await _currentUserService.GetUser();
           
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
    }
}