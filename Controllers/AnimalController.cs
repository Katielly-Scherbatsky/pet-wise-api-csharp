using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly ILogger<AnimalController> _logger;
        private readonly AppDbContext _context;

        public AnimalController(ILogger<AnimalController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var animais = await _context.Animal.ToListAsync();

            if (animais == null || animais.Count == 0)
            {
                return NotFound("Nenhum animal encontrado.");
            }

            return Ok(animais);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var animal = await _context.Animal.FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                return NotFound($"Animal com ID {id} não encontrado.");
            }

            return Ok(animal);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AnimalDto dto)
        {
            var animal = new AnimalModel
            {
                Nome = dto.Nome,
                DataNascimento = dto.DataNascimento,
                UsuarioId = dto.UsuarioId,
            };

            _context.Animal.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = animal.Id }, animal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AnimalDto dto)
        {
            var animal = await _context.Animal.FindAsync(id);

            if (animal == null)
            {
                return NotFound($"Animal com ID {id} não encontrado.");
            }

            animal.Nome = dto.Nome;
            animal.DataNascimento = dto.DataNascimento;
            animal.UsuarioId = dto.UsuarioId;

            await _context.SaveChangesAsync();

            return Ok(animal);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _context.Animal.FindAsync(id);

            if (animal == null)
            {
                return NotFound($"Animal com ID {id} não encontrado.");
            }

            _context.Animal.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
