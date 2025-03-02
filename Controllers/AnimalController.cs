using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [Authorize]
    public class AnimalController : BaseController
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
            try
            {
                int usuarioId = GetIdUsuarioLogado();
                var animais = await _context.Animal
                    .Include(x => x.Usuario)
                    .Where(x => x.UsuarioId == usuarioId)
                    .ToListAsync();

                return Ok(animais);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar Animais");
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                int usuarioId = GetIdUsuarioLogado();
                var animal = await _context.Animal
                    .Include(x => x.Usuario)
                    .FirstOrDefaultAsync(a => a.Id == id && a.UsuarioId == usuarioId);

                if (animal == null)
                {
                    _logger.LogWarning("Animal com ID {Id} não encontrado.", id);
                    return NotFound($"Animal com ID {id} não encontrado.");
                }

                return Ok(animal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar Animal pelo Id {Id}", id);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AnimalDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados do animal são obrigatórios.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int usuarioId = GetIdUsuarioLogado();

                var animal = new AnimalModel
                {
                    Nome = dto.Nome,
                    DataNascimento = dto.DataNascimento,
                    UsuarioId = usuarioId,
                };

                _context.Animal.Add(animal);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = animal.Id }, animal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao criar Animal");
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] AnimalDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados do animal são obrigatórios.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int usuarioId = GetIdUsuarioLogado();
                var animal = await _context.Animal.FirstOrDefaultAsync(x => x.Id == id && x.UsuarioId == usuarioId);
                if (animal == null)
                {
                    return NotFound($"Animal com Id {id} não encontrado.");
                }

                animal.Nome = dto.Nome;
                animal.DataNascimento = dto.DataNascimento;

                await _context.SaveChangesAsync();

                return Ok(animal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao atualizar Animal Id: {Id}", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int usuarioId = GetIdUsuarioLogado();
                var animal = await _context.Animal.FirstOrDefaultAsync(x => x.Id == id && x.UsuarioId == usuarioId);
                if (animal == null)
                {
                    return NotFound($"Animal com Id {id} não encontrado.");
                }

                _context.Animal.Remove(animal);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao excluir Banho e Tosa Id: {Id}", id);
                return BadRequest();
            }
        }
    }
}
