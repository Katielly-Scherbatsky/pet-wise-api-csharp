using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("suplementacao")]
    public class SuplementacaoController : ControllerBase
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
            var suplementacoes = await _context.Suplementacao.Include(v => v.Animal).ToListAsync();

            if (suplementacoes == null || suplementacoes.Count == 0)
            {
                return NotFound("Nenhum dado encontrado.");
            }

            return Ok(suplementacoes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var suplementacao = await _context.Suplementacao.Include(v => v.Animal).FirstOrDefaultAsync(a => a.Id == id);

            if (suplementacao == null)
            {
                return NotFound($"Suplementação com ID {id} não encontrado.");
            }

            return Ok(suplementacao);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SuplementacaoDto dto)
        {
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SuplementacaoDto dto)
        {
            var suplementacao = await _context.Suplementacao.FindAsync(id);

            if (suplementacao == null)
            {
                return NotFound($"Suplementação com ID {id} não encontrado.");
            }

            suplementacao.TipoSuplementacao = dto.TipoSuplementacao;
            suplementacao.DataAplicacao = dto.DataAplicacao;
            suplementacao.DataProximaAplicacao = dto.DataProximaAplicacao;
            suplementacao.Observacoes = dto.Observacoes;
            suplementacao.AnimalId = dto.AnimalId;

            await _context.SaveChangesAsync();

            return Ok(suplementacao);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var suplementacao = await _context.Suplementacao.FindAsync(id);

            if (suplementacao == null)
            {
                return NotFound($"Suplementação com ID {id} não encontrado.");
            }

            _context.Suplementacao.Remove(suplementacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
