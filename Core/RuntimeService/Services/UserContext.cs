using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Persistence.Profile.Types.DTO;
using ProfileService;
using RuntimeService.Controllers.Models;

namespace RuntimeService.Services;

internal class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly IProfileService _profileService;

    public UserContext(
        IHttpContextAccessor httpContextAccessor,
        IUserService userService,
        IProfileService profileService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _profileService = profileService;
    }

    private ClaimsPrincipal? ClaimsPrincipal => _httpContextAccessor.HttpContext?.User;
    
    public string? BearerToken
    {
        get
        {
            if (_httpContextAccessor.HttpContext == null) return null;

            var authHeaders = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];

            if (authHeaders.Count == 0) return null;
            if (authHeaders.Count > 1) throw new Exception("Too many authorization headers sent");

            return authHeaders[0];
        }
    }
    
    public async Task<ProfileDTO> GetUserProfile()
    {
        return await GetUserProfileOrDefault() ??
                      throw new InvalidOperationException("Could not find user profile");
    }
    
    public async Task<ProfileDTO?> GetUserProfileOrDefault()
    {
        if(BearerToken == null || ClaimsPrincipal == null) throw new UnauthorizedAccessException();

        var oidcUser = await _userService.GetUser(ClaimsPrincipal, BearerToken);
        return await _profileService.GetByExternalId(oidcUser.Id);
    }

    public async Task<ProfileDTO> CreateProfileForUser(ProfileInput input)
    {
        if(BearerToken == null || ClaimsPrincipal == null) throw new UnauthorizedAccessException();

        var currentProfile = await GetUserProfileOrDefault();
        if (currentProfile != null)
        {
            throw new InvalidOperationException("A profile already exists for this user");
        }

        var currentUser = await _userService.GetUser(ClaimsPrincipal, BearerToken);
           
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
        return profile;
    }
}
