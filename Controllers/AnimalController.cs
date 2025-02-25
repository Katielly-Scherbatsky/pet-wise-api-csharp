using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var animais = await _context.Animal
                .Include(x => x.Usuario)
                .ToListAsync();

            return Ok(animais);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var animal = await _context.Animal
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                _logger.LogWarning("Animal com ID {Id} não encontrado.", id);
                return NotFound($"Animal com ID {id} não encontrado.");
            }

            return Ok(animal);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AnimalDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados do animal são obrigatórios.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioExiste = await _context.Usuario.AnyAsync(x => x.Id == dto.UsuarioId);
            if (!usuarioExiste)
            {
                _logger.LogWarning("Usuário com ID {UsuarioId} não encontrado.", dto.UsuarioId);
                return BadRequest($"Usuário com ID {dto.UsuarioId} não encontrado.");
            }

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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] AnimalDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados do animal são obrigatórios.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                _logger.LogWarning("Animal com ID {Id} não encontrado.", id);
                return NotFound($"Animal com ID {id} não encontrado.");
            }

            if (animal.UsuarioId != dto.UsuarioId)
            {
                var usuarioExiste = await _context.Usuario.AnyAsync(x => x.Id == dto.UsuarioId);
                if (!usuarioExiste)
                {
                    _logger.LogWarning("Usuário com ID {UsuarioId} não encontrado.", dto.UsuarioId);
                    return BadRequest($"Usuário com ID {dto.UsuarioId} não encontrado.");
                }
            }

            animal.Nome = dto.Nome;
            animal.DataNascimento = dto.DataNascimento;
            animal.UsuarioId = dto.UsuarioId;

            await _context.SaveChangesAsync();

            return Ok(animal);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                _logger.LogWarning("Animal com ID {Id} não encontrado.", id);
                return NotFound($"Animal com ID {id} não encontrado.");
            }

            _context.Animal.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
