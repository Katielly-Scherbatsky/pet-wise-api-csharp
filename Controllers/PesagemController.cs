using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [Authorize]
    public class PesagemController : BaseController
    {
        private readonly ILogger<PesagemController> _logger;
        private readonly AppDbContext _context;

        public PesagemController(ILogger<PesagemController> logger, AppDbContext context)
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
                var pesagens = await _context.Pesagem
                .Include(p => p.Animal)
                .Where(p => p.Animal.UsuarioId == usuarioId)
                .ToListAsync();

                return Ok(pesagens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar pesagens");
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                int usuarioId = GetIdUsuarioLogado();
                var pesagem = await _context.Pesagem
                    .Include(p => p.Animal)
                    .FirstOrDefaultAsync(p => p.Id == id && p.Animal.UsuarioId == usuarioId);

                if (pesagem == null)
                {
                    _logger.LogWarning("Pesagem com ID {Id} não encontrada.", id);
                    return NotFound($"Pesagem com ID {id} não encontrada.");
                }

                return Ok(pesagem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar pesagem pelo Id {Id}", id);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PesagemDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados da pesagem são obrigatórios.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int usuarioId = GetIdUsuarioLogado();
                var animalExiste = await _context.Animal.AnyAsync(a => a.Id == dto.AnimalId && a.UsuarioId == usuarioId);
                if (!animalExiste)
                {
                    _logger.LogWarning("Animal com ID {AnimalId} não encontrado.", dto.AnimalId);
                    return BadRequest($"Animal com ID {dto.AnimalId} não encontrado.");
                }

                var pesagem = new PesagemModel
                {
                    Peso = dto.Peso,
                    DataPesagem = dto.DataPesagem,
                    Observacoes = dto.Observacoes,
                    AnimalId = dto.AnimalId,
                };

                _context.Pesagem.Add(pesagem);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = pesagem.Id }, pesagem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao criar pesagem");
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] PesagemDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados da pesagem são obrigatórios.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int usuarioId = GetIdUsuarioLogado();
                var pesagem = await _context.Pesagem.FindAsync(id);
                if (pesagem == null)
                {
                    _logger.LogWarning("Pesagem com ID {Id} não encontrada.", id);
                    return NotFound($"Pesagem com ID {id} não encontrada.");
                }

                if (pesagem.AnimalId != dto.AnimalId)
                {
                    var animalExiste = await _context.Animal.AnyAsync(a => a.Id == dto.AnimalId && a.UsuarioId == usuarioId);
                    if (!animalExiste)
                    {
                        _logger.LogWarning("Animal com ID {AnimalId} não encontrado.", dto.AnimalId);
                        return BadRequest($"Animal com ID {dto.AnimalId} não encontrado.");
                    }
                }

                pesagem.Peso = dto.Peso;
                pesagem.DataPesagem = dto.DataPesagem;
                pesagem.Observacoes = dto.Observacoes;
                pesagem.AnimalId = dto.AnimalId;

                await _context.SaveChangesAsync();

                return Ok(pesagem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao atualizar pessagem com Id: {Id}", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int usuarioId = GetIdUsuarioLogado();
                var pesagem = await _context.Pesagem
                    .Include(p => p.Animal)
                    .FirstOrDefaultAsync(p => p.Id == id && p.Animal.UsuarioId == usuarioId);
                if (pesagem == null)
                {
                    _logger.LogWarning("Pesagem com ID {Id} não encontrada.", id);
                    return NotFound($"Pesagem com ID {id} não encontrada.");
                }

                _context.Pesagem.Remove(pesagem);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao excluir pesagem com Id: {Id}", id);
                return BadRequest();
            }
        }
    }
}
