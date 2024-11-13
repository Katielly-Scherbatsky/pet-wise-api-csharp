using Microsoft.AspNetCore.Mvc;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PesagemController : ControllerBase
    {

        private readonly ILogger<PesagemController> _logger;

        public PesagemController(ILogger<PesagemController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new List<PesagemModel>());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(new PesagemModel());
        }

        [HttpPost]
        public IActionResult Post([FromBody] PesagemDto dto)
        {
            return Ok(new PesagemModel());
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] PesagemDto dto)
        {
            return Ok(new PesagemModel());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }


    }
}
