using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TratamentoController : ControllerBase
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
            var tratamentos = await _context.Tratamento
                .Include(t => t.Animal)
                .ToListAsync();

            return Ok(tratamentos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tratamento = await _context.Tratamento
                .Include(t => t.Animal)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tratamento == null)
            {
                _logger.LogWarning("Tratamento com ID {Id} não encontrado.", id);
                return NotFound($"Tratamento com ID {id} não encontrado.");
            }

            return Ok(tratamento);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TratamentoDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados de tratamento são obrigatórios.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var animalExiste = await _context.Animal.AnyAsync(a => a.Id == dto.AnimalId);
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] TratamentoDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados de tratamento são obrigatórios.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tratamento = await _context.Tratamento.FindAsync(id);
            if (tratamento == null)
            {
                _logger.LogWarning("Tratamento com ID {Id} não encontrado.", id);
                return NotFound($"Tratamento com ID {id} não encontrado.");
            }

            if (tratamento.AnimalId != dto.AnimalId)
            {
                var animalExiste = await _context.Animal.AnyAsync(a => a.Id == dto.AnimalId);
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tratamento = await _context.Tratamento.FindAsync(id);
            if (tratamento == null)
            {
                _logger.LogWarning("Tratamento com ID {Id} não encontrado.", id);
                return NotFound($"Tratamento com ID {id} não encontrado.");
            }

            _context.Tratamento.Remove(tratamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
