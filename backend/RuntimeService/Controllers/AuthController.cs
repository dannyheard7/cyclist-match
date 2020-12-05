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
        private readonly IExternalUserService _externalUserService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;
        
        public AuthController(IExternalUserService externalUserService, IUserRepository userRepository, ICurrentUserService currentUserService)
        {
            _externalUserService = externalUserService ?? throw new ArgumentNullException(nameof(externalUserService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
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
            await _userRepository.DeleteUser(currentUser);

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