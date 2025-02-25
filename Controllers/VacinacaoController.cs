using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var vacinacoes = await _context.Vacinacao
                .Include(v => v.Animal)
                .ToListAsync();

            return Ok(vacinacoes);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vacinacao = await _context.Vacinacao
                .Include(v => v.Animal)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (vacinacao == null)
            {
                _logger.LogWarning("Vacinação com ID {Id} não encontrada.", id);
                return NotFound($"Vacinação com ID {id} não encontrada.");
            }

            return Ok(vacinacao);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VacinacaoDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados da vacinação são obrigatórios.");
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] VacinacaoDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados da vacinação são obrigatórios.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vacinacao = await _context.Vacinacao.FindAsync(id);
            if (vacinacao == null)
            {
                _logger.LogWarning("Vacinação com ID {Id} não encontrada.", id);
                return NotFound($"Vacinação com ID {id} não encontrada.");
            }

            if (vacinacao.AnimalId != dto.AnimalId)
            {
                var animalExiste = await _context.Animal.AnyAsync(a => a.Id == dto.AnimalId);
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vacinacao = await _context.Vacinacao.FindAsync(id);
            if (vacinacao == null)
            {
                _logger.LogWarning("Vacinação com ID {Id} não encontrada.", id);
                return NotFound($"Vacinação com ID {id} não encontrada.");
            }

            _context.Vacinacao.Remove(vacinacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
