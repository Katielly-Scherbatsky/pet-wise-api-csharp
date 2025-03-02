using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [Authorize]
    public class SuplementacaoController : BaseController
    {
        private readonly ILogger<SuplementacaoController> _logger;
        private readonly AppDbContext _context;

        public SuplementacaoController(ILogger<SuplementacaoController> logger, AppDbContext context)
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
                var suplementacoes = await _context.Suplementacao
                .Include(s => s.Animal)
                .Where(s => s.Animal.UsuarioId == usuarioId)
                .ToListAsync();

                return Ok(suplementacoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar suplementação");
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                int usuarioId = GetIdUsuarioLogado();
                var suplementacao = await _context.Suplementacao
                .Include(s => s.Animal)
                .FirstOrDefaultAsync(s => s.Id == id && s.Animal.UsuarioId == usuarioId);

                if (suplementacao == null)
                {
                    _logger.LogWarning("Suplementação com ID {Id} não encontrada.", id);
                    return NotFound($"Suplementação com ID {id} não encontrada.");
                }

                return Ok(suplementacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar suplementação pelo Id {Id}", id);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SuplementacaoDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados de suplementação são obrigatórios.");
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

                var suplementacao = new SuplementacaoModel
                {
                    TipoSuplementacao = dto.TipoSuplementacao,
                    NumeroDoses = dto.NumeroDoses,
                    DataAplicacao = dto.DataAplicacao,
                    DataProximaAplicacao = dto.DataProximaAplicacao,
                    Observacoes = dto.Observacoes,
                    AnimalId = dto.AnimalId,
                };

                _context.Suplementacao.Add(suplementacao);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = suplementacao.Id }, suplementacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao criar suplementação");
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] SuplementacaoDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados de suplementação são obrigatórios.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int usuarioId = GetIdUsuarioLogado();
                var suplementacao = await _context.Suplementacao
                    .Include(s => s.Animal)
                    .FirstOrDefaultAsync(s => s.Id == id && s.Animal.UsuarioId == usuarioId);

                if (suplementacao == null)
                {
                    _logger.LogWarning("Suplementação com ID {Id} não encontrada.", id);
                    return NotFound($"Suplementação com ID {id} não encontrada.");
                }

                if (suplementacao.AnimalId != dto.AnimalId)
                {
                    var animalExiste = await _context.Animal.AnyAsync(a => a.Id == dto.AnimalId && a.UsuarioId == usuarioId);
                    if (!animalExiste)
                    {
                        _logger.LogWarning("Animal com ID {AnimalId} não encontrado.", dto.AnimalId);
                        return BadRequest($"Animal com ID {dto.AnimalId} não encontrado.");
                    }
                }

                suplementacao.TipoSuplementacao = dto.TipoSuplementacao;
                suplementacao.NumeroDoses = dto.NumeroDoses;
                suplementacao.DataAplicacao = dto.DataAplicacao;
                suplementacao.DataProximaAplicacao = dto.DataProximaAplicacao;
                suplementacao.Observacoes = dto.Observacoes;
                suplementacao.AnimalId = dto.AnimalId;

                await _context.SaveChangesAsync();

                return Ok(suplementacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao atualizar suplementação com Id: {Id}", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int usuarioId = GetIdUsuarioLogado();
                var suplementacao = await _context.Suplementacao
                    .Include(s => s.Animal)
                    .FirstOrDefaultAsync(s => s.Id == id && s.Animal.UsuarioId == usuarioId);

                if (suplementacao == null)
                {
                    _logger.LogWarning("Suplementação com ID {Id} não encontrada.", id);
                    return NotFound($"Suplementação com ID {id} não encontrada.");
                }

                _context.Suplementacao.Remove(suplementacao);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao excluir suplementação com Id: {Id}", id);
                return BadRequest();
            }
        }
    }
}
