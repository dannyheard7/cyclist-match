using System.Threading.Tasks;
using Auth;
using ChatService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Profile.Types.DTO;
using RuntimeService.Services;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationUserService _authenticationUserService;
        private readonly IUserContext _userContext;
        private readonly IProfileService _profileService;
        private readonly IChatClientFactory _chatClientFactory;
        
        public AuthController(
            IAuthenticationUserService authenticationUserService,
            IUserContext userContext,
            IProfileService profileService,
            IChatClientFactory chatClientFactory)
        {
            _authenticationUserService = authenticationUserService;
            _userContext = userContext;
            _profileService = profileService;
            _chatClientFactory = chatClientFactory;
        }

        [HttpGet("user")]
        public async Task<ActionResult<ProfileDTO>> Login([FromHeader]string authorization)
        {
            var user = await _authenticationUserService.GetUser(User, authorization);
            var profile = await _profileService.GetByOidcUser(user);

            if (profile == null)
            {
                return NotFound();
            }
            
            var client = _chatClientFactory.GetClient();
            await client.GetConversations();

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