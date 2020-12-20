using System;
using System.Threading.Tasks;
using Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository;
using RuntimeService.Services;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("auth")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IExternalUserService _externalUserService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        
        public AuthController(IExternalUserService externalUserService, ICurrentUserService currentUserService, IUserRepository userRepository, IUserService userService)
        {
            _externalUserService = externalUserService;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromHeader]string authorization)
        {
            var user = await _externalUserService.GetUser(User, authorization);
            var updateResult = await _userRepository.UpdateUserDetails(user);

            if (!updateResult) return StatusCode(StatusCodes.Status500InternalServerError);

            var hasProfile = await _userRepository.ExternalUserHasProfile(user.ExternalId);
            return Ok(new LoginResponse(hasProfile));
        }
        
        [HttpGet("user")]
        public async Task<ActionResult> GetUserDetails()
        {
            return Ok(await _currentUserService.GetUser());
        }
        
        [HttpDelete("user")]
        public async Task<ActionResult> DeleteUser()
        {
            var currentUser = await _currentUserService.GetUser();
            await _userService.DeleteUser(currentUser);

            return NoContent();
        }

        private class LoginResponse
        {
            public LoginResponse(bool hasProfile)
            {
                HasProfile = hasProfile;
            }

            public bool HasProfile { get; }
        }
    }
}