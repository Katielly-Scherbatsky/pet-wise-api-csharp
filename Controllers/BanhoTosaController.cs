using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [Authorize]
    public class BanhoTosaController : BaseController
    {
        private readonly ILogger<BanhoTosaController> _logger;
        private readonly AppDbContext _context;

        public BanhoTosaController(ILogger<BanhoTosaController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuarioId = GetIdUsuarioLogado();
                var registros = await _context.BanhoTosa
                    .Include(v => v.Animal)
                    .Where(v => v.Animal.UsuarioId == usuarioId)
                    .ToListAsync();

                return Ok(registros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar Banho e Tosa");
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var usuarioId = GetIdUsuarioLogado();
                var registro = await _context.BanhoTosa
                    .Include(v => v.Animal)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (registro == null)
                {
                    _logger.LogWarning("Banho e Tosa com ID {Id} não encontrado.", id);
                    return NotFound($"Banho e Tosa com ID {id} não encontrado.");
                }

                return Ok(registro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar Banho e Tosa pelo Id {Id}", id);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BanhoTosaDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados de Banho e Tosa são obrigatórios.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var usuarioId = GetIdUsuarioLogado();
                var animalExiste = await _context.Animal.AnyAsync(a => a.Id == dto.AnimalId && a.UsuarioId == usuarioId);
                if (!animalExiste)
                {
                    _logger.LogWarning("Animal com ID {AnimalId} não encontrado.", dto.AnimalId);
                    return BadRequest($"Animal com ID {dto.AnimalId} não encontrado.");
                }

                var banhoTosa = new BanhoTosaModel
                {
                    Executor = dto.Executor,
                    DataServico = dto.DataServico,
                    Observacoes = dto.Observacoes,
                    AnimalId = dto.AnimalId
                };

                _context.BanhoTosa.Add(banhoTosa);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = banhoTosa.Id }, banhoTosa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao criar Banho e Tosa");
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] BanhoTosaDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados de Banho e Tosa são obrigatórios.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var usuarioId = GetIdUsuarioLogado();
                var banhoTosa = await _context.BanhoTosa
                    .Include(v => v.Animal)
                    .FirstOrDefaultAsync(v => v.Id == id && v.Animal.UsuarioId == usuarioId);

                if (banhoTosa == null)
                {
                    _logger.LogWarning("Banho e Tosa com ID {Id} não encontrado.", id);
                    return NotFound($"Banho e Tosa com ID {id} não encontrado.");
                }

                if (banhoTosa.AnimalId != dto.AnimalId)
                {
                    var animalExiste = await _context.Animal.AnyAsync(a => a.Id == dto.AnimalId && a.UsuarioId == usuarioId);
                    if (!animalExiste)
                    {
                        _logger.LogWarning("Animal com ID {AnimalId} não encontrado.", dto.AnimalId);
                        return BadRequest($"Animal com ID {dto.AnimalId} não encontrado.");
                    }
                }
                banhoTosa.Executor = dto.Executor;
                banhoTosa.DataServico = dto.DataServico;
                banhoTosa.Observacoes = dto.Observacoes;
                banhoTosa.AnimalId = dto.AnimalId;

                await _context.SaveChangesAsync();

                return Ok(banhoTosa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao atualizar Banho e Tosa Id: {Id}", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var usuarioId = GetIdUsuarioLogado();
                var banhoTosa = await _context.BanhoTosa
                    .Include(v => v.Animal)
                    .FirstOrDefaultAsync(v => v.Id == id && v.Animal.UsuarioId == usuarioId);

                if (banhoTosa == null)
                {
                    _logger.LogWarning("Banho e Tosa com ID {Id} não encontrado.", id);
                    return NotFound($"Banho e Tosa com ID {id} não encontrado.");
                }

                _context.BanhoTosa.Remove(banhoTosa);
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
