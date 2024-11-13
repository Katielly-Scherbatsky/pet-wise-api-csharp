using Microsoft.AspNetCore.Mvc;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new List<UsuarioModel>());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(new UsuarioModel());
        }

        [HttpPost]
        public IActionResult Post([FromBody] UsuarioDto dto)
        {
            return Ok(new UsuarioModel());
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UsuarioDto dto)
        {
            return Ok(new UsuarioModel());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }


    }
}
