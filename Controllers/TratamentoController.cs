using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("tratamento")]
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
            var tratamentos = await _context.Tratamento.Include(v => v.Animal).ToListAsync();

            if (tratamentos == null || tratamentos.Count == 0)
            {
                return NotFound("Nenhum dado encontrado.");
            }

            return Ok(tratamentos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tratamento = await _context.Tratamento.Include(v => v.Animal).FirstOrDefaultAsync(a => a.Id == id);

            if (tratamento == null)
            {
                return NotFound($"Tratamento com ID {id} não encontrado.");
            }

            return Ok(tratamento);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TratamentoDto dto)
        {
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TratamentoDto dto)
        {
            var tratamento = await _context.Tratamento.FindAsync(id);

            if (tratamento == null)
            {
                return NotFound($"Tratamento com ID {id} não encontrado.");
            }

            tratamento.TipoTratamento = dto.TipoTratamento;
            tratamento.DataAplicacao = dto.DataAplicacao;
            tratamento.DataProximaAplicacao = dto.DataProximaAplicacao;
            tratamento.Observacoes = dto.Observacoes;
            tratamento.AnimalId = dto.AnimalId;

            await _context.SaveChangesAsync();

            return Ok(tratamento);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tratamento = await _context.Tratamento.FindAsync(id);

            if (tratamento == null)
            {
                return NotFound($"Tratamento com ID {id} não encontrado.");
            }

            _context.Tratamento.Remove(tratamento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
