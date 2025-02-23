using Microsoft.AspNetCore.Mvc;
using Pet.Wise.Api.Dto;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            return Unauthorized();
        }
    }
}
