using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("secure")]
        public IActionResult GetSecureData()
        {
            var userId = User.Identity?.Name ?? "Unknown";
            return Ok($"You are authenticated. User: {userId}");
        }
    }
}
