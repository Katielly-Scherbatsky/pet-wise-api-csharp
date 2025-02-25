using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PesagemController : ControllerBase
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
            var pesagens = await _context.Pesagem
                .Include(p => p.Animal)
                .ToListAsync();

            return Ok(pesagens);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pesagem = await _context.Pesagem
                .Include(p => p.Animal)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pesagem == null)
            {
                _logger.LogWarning("Pesagem com ID {Id} não encontrada.", id);
                return NotFound($"Pesagem com ID {id} não encontrada.");
            }

            return Ok(pesagem);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PesagemDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados da pesagem são obrigatórios.");
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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] PesagemDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados da pesagem são obrigatórios.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pesagem = await _context.Pesagem.FindAsync(id);
            if (pesagem == null)
            {
                _logger.LogWarning("Pesagem com ID {Id} não encontrada.", id);
                return NotFound($"Pesagem com ID {id} não encontrada.");
            }

            if (pesagem.AnimalId != dto.AnimalId)
            {
                var animalExiste = await _context.Animal.AnyAsync(a => a.Id == dto.AnimalId);
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pesagem = await _context.Pesagem.FindAsync(id);
            if (pesagem == null)
            {
                _logger.LogWarning("Pesagem com ID {Id} não encontrada.", id);
                return NotFound($"Pesagem com ID {id} não encontrada.");
            }

            _context.Pesagem.Remove(pesagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
