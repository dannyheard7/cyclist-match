using Microsoft.AspNetCore.Mvc;

namespace RuntimeService.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetHealth()
        {
            return Ok();
        }
    }
}