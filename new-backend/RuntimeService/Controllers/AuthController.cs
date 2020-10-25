using System;
using System.Threading.Tasks;
using Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("auth")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;
        
        public AuthController(ICurrentUserService currentUserService, IUserRepository userRepository)
        {
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login()
        {
            var user = await _currentUserService.GetUser();
            var updateResult = await _userRepository.UpdateUserDetails(user);

            if (!updateResult) return StatusCode(StatusCodes.Status500InternalServerError);

            var hasProfile = await _userRepository.ExternalUserHasProfile(user.ExternalId);
            return Ok(new LoginResponse(hasProfile));
        }
        
        [HttpGet("user")]
        public async Task<ActionResult> GetUserDetails()
        {
            var userId = await _currentUserService.GetExternalUserId();
            var user = await _userRepository.GetUserDetails(userId);
            return Ok(user);
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