using System.Threading.Tasks;
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
        private readonly IUserContext _userContext;
        
        public AuthController(IUserContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet("user")]
        public async Task<ActionResult<ProfileDTO>> Login()
        {
            var profile = await _userContext.GetUserProfileOrDefault();

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }
    }
}