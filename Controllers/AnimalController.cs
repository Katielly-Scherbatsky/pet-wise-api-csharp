using Microsoft.AspNetCore.Mvc;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnimalController : ControllerBase
    {

        private readonly ILogger<AnimalController> _logger;

        public AnimalController(ILogger<AnimalController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new List<AnimalModel>());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(new AnimalModel());
        }

        [HttpPost]
        public IActionResult Post([FromBody] AnimalDto dto)
        {
            return Ok(new AnimalModel());
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AnimalDto dto)
        {
            return Ok(new AnimalModel());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return NoContent();
        }


    }
}
