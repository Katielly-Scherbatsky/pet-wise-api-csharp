using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [Authorize]
    public class VacinacaoController : BaseController
    {
        private readonly ILogger<VacinacaoController> _logger;
        private readonly AppDbContext _context;

        public VacinacaoController(ILogger<VacinacaoController> logger, AppDbContext context)
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
                var vacinacoes = await _context.Vacinacao
                .Include(v => v.Animal)
                .Where(v => v.Animal.UsuarioId == usuarioId)
                .ToListAsync();

                return Ok(vacinacoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar vacinações");
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                int usuarioId = GetIdUsuarioLogado();
                var vacinacao = await _context.Vacinacao
                .Include(v => v.Animal)
                .FirstOrDefaultAsync(v => v.Id == id && v.Animal.UsuarioId == usuarioId);

                if (vacinacao == null)
                {
                    _logger.LogWarning("Vacinação com ID {Id} não encontrada.", id);
                    return NotFound($"Vacinação com ID {id} não encontrada.");
                }

                return Ok(vacinacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar vacinação pelo Id {Id}", id);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VacinacaoDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados da vacinação são obrigatórios.");
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

                var vacinacao = new VacinacaoModel
                {
                    NomeVacina = dto.NomeVacina,
                    DataAplicacao = dto.DataAplicacao,
                    DataProximaAplicacao = dto.DataProximaAplicacao,
                    Observacoes = dto.Observacoes,
                    AnimalId = dto.AnimalId,
                };

                _context.Vacinacao.Add(vacinacao);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = vacinacao.Id }, vacinacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao criar vacinação");
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] VacinacaoDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados da vacinação são obrigatórios.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int usuarioId = GetIdUsuarioLogado();
                var vacinacao = await _context.Vacinacao
                    .Include(v => v.Animal)
                    .FirstOrDefaultAsync(v => v.Id == id && v.Animal.UsuarioId == usuarioId);
                if (vacinacao == null)
                {
                    _logger.LogWarning("Vacinação com ID {Id} não encontrada.", id);
                    return NotFound($"Vacinação com ID {id} não encontrada.");
                }

                if (vacinacao.AnimalId != dto.AnimalId)
                {
                    var animalExiste = await _context.Animal.AnyAsync(a => a.Id == dto.AnimalId && a.UsuarioId == usuarioId);
                    if (!animalExiste)
                    {
                        _logger.LogWarning("Animal com ID {AnimalId} não encontrado.", dto.AnimalId);
                        return BadRequest($"Animal com ID {dto.AnimalId} não encontrado.");
                    }
                }

                vacinacao.NomeVacina = dto.NomeVacina;
                vacinacao.DataAplicacao = dto.DataAplicacao;
                vacinacao.DataProximaAplicacao = dto.DataProximaAplicacao;
                vacinacao.Observacoes = dto.Observacoes;
                vacinacao.AnimalId = dto.AnimalId;

                await _context.SaveChangesAsync();

                return Ok(vacinacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao atualizar vacinação com Id: {Id}", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int usuarioId = GetIdUsuarioLogado();
                var vacinacao = await _context.Vacinacao
                    .Include(t => t.Animal)
                    .FirstOrDefaultAsync(t => t.Id == id && t.Animal.UsuarioId == usuarioId);
                if (vacinacao == null)
                {
                    _logger.LogWarning("Vacinação com ID {Id} não encontrada.", id);
                    return NotFound($"Vacinação com ID {id} não encontrada.");
                }

                _context.Vacinacao.Remove(vacinacao);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao excluir vacinação com Id: {Id}", id);
                return BadRequest();
            }
        }
    }
}
