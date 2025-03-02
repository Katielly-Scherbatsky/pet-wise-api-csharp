using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [Authorize]
    public class TratamentoController : BaseController
    {
        private readonly ILogger<TratamentoController> _logger;
        private readonly AppDbContext _context;

        public TratamentoController(ILogger<TratamentoController> logger, AppDbContext context)
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
                var tratamentos = await _context.Tratamento
                .Include(t => t.Animal)
                .Where(t => t.Animal.UsuarioId == usuarioId)
                .ToListAsync();

                return Ok(tratamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar tratamentos");
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                int usuarioId = GetIdUsuarioLogado();
                var tratamento = await _context.Tratamento
                .Include(t => t.Animal)
                .FirstOrDefaultAsync(t => t.Id == id && t.Animal.UsuarioId == usuarioId);

                if (tratamento == null)
                {
                    _logger.LogWarning("Tratamento com ID {Id} não encontrado.", id);
                    return NotFound($"Tratamento com ID {id} não encontrado.");
                }

                return Ok(tratamento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar tratamento pelo Id {Id}", id);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TratamentoDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados de tratamento são obrigatórios.");
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

                var tratamento = new TratamentoModel
                {
                    TipoTratamento = dto.TipoTratamento,
                    DataAplicacao = dto.DataAplicacao,
                    DataProximaAplicacao = dto.DataProximaAplicacao,
                    Observacoes = dto.Observacoes,
                    AnimalId = dto.AnimalId,
                };

                _context.Tratamento.Add(tratamento);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = tratamento.Id }, tratamento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao criar tratamento");
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] TratamentoDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados de tratamento são obrigatórios.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int usuarioId = GetIdUsuarioLogado();
                var tratamento = await _context.Tratamento
                    .Include(t => t.Animal)
                    .FirstOrDefaultAsync(t => t.Id == id && t.Animal.UsuarioId == usuarioId);
                if (tratamento == null)
                {
                    _logger.LogWarning("Tratamento com ID {Id} não encontrado.", id);
                    return NotFound($"Tratamento com ID {id} não encontrado.");
                }

                if (tratamento.AnimalId != dto.AnimalId)
                {
                    var animalExiste = await _context.Animal.AnyAsync(a => a.Id == dto.AnimalId && a.UsuarioId == usuarioId);
                    if (!animalExiste)
                    {
                        _logger.LogWarning("Animal com ID {AnimalId} não encontrado.", dto.AnimalId);
                        return BadRequest($"Animal com ID {dto.AnimalId} não encontrado.");
                    }
                }

                tratamento.TipoTratamento = dto.TipoTratamento;
                tratamento.DataAplicacao = dto.DataAplicacao;
                tratamento.DataProximaAplicacao = dto.DataProximaAplicacao;
                tratamento.Observacoes = dto.Observacoes;
                tratamento.AnimalId = dto.AnimalId;

                await _context.SaveChangesAsync();

                return Ok(tratamento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao atualizar tratamento com Id: {Id}", id);
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int usuarioId = GetIdUsuarioLogado();
                var tratamento = await _context.Tratamento
                    .Include(t => t.Animal)
                    .FirstOrDefaultAsync(t => t.Id == id && t.Animal.UsuarioId == usuarioId);
                if (tratamento == null)
                {
                    _logger.LogWarning("Tratamento com ID {Id} não encontrado.", id);
                    return NotFound($"Tratamento com ID {id} não encontrado.");
                }

                _context.Tratamento.Remove(tratamento);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao excluir tratamento com Id: {Id}", id);
                return BadRequest();
            }
        }
    }
}
