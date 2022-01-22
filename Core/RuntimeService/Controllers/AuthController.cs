using System;
using System.Threading.Tasks;
using Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository;
using Persistence.Types.DTO;
using RuntimeService.Services;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationUserService _authenticationUserService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProfileService _profileService;
        
        public AuthController(IAuthenticationUserService authenticationUserService, ICurrentUserService currentUserService, IProfileService profileService)
        {
            _authenticationUserService = authenticationUserService;
            _currentUserService = currentUserService;
            _profileService = profileService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ProfileDTO>> Login([FromHeader]string authorization)
        {
            var user = await _authenticationUserService.GetUser(User, authorization);
            var profile = await _profileService.GetByOidcUser(user);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }
        
        [HttpGet("user")]
        public async Task<ActionResult> GetUserDetails()
        {
            return Ok(await _currentUserService.GetUser());
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