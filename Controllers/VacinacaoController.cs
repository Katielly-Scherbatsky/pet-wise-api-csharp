using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VacinacaoController : ControllerBase
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
            var vacinacoes = await _context.Vacinacao.Include(v => v.Animal).ToListAsync();

            if (vacinacoes == null || vacinacoes.Count == 0)
            {
                return NotFound("Nenhum animal encontrado.");
            }

            return Ok(vacinacoes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vacinacao = await _context.Vacinacao.Include(v => v.Animal).FirstOrDefaultAsync(a => a.Id == id);

            if (vacinacao == null)
            {
                return NotFound($"Vacinação com ID {id} não encontrado.");
            }

            return Ok(vacinacao);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VacinacaoDto dto)
        {
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] VacinacaoDto dto)
        {
            var vacinacao = await _context.Vacinacao.FindAsync(id);

            if (vacinacao == null)
            {
                return NotFound($"Vacinação com ID {id} não encontrado.");
            }

            vacinacao.NomeVacina = dto.NomeVacina;
            vacinacao.DataAplicacao = dto.DataAplicacao;
            vacinacao.DataProximaAplicacao = dto.DataProximaAplicacao;
            vacinacao.Observacoes = dto.Observacoes;
            vacinacao.AnimalId = dto.AnimalId;

            await _context.SaveChangesAsync();

            return Ok(vacinacao);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vacinacao = await _context.Vacinacao.FindAsync(id);

            if (vacinacao == null)
            {
                return NotFound($"Vacinação com ID {id} não encontrado.");
            }

            _context.Vacinacao.Remove(vacinacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
