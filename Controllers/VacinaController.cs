using Microsoft.AspNetCore.Mvc;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VacinaController : ControllerBase
    {

        private readonly ILogger<VacinaController> _logger;

        public VacinaController(ILogger<VacinaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new List<VacinaModel>());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(new VacinaModel());
        }

        [HttpPost]
        public IActionResult Post([FromBody] VacinaDto dto)
        {
            return Ok(new VacinaModel());
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] VacinaDto dto)
        {
            return Ok(new VacinaModel());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }


    }
}
