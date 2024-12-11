using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            var pesagens = await _context.Pesagem.ToListAsync();

            if (pesagens == null || pesagens.Count == 0)
            {
                return NotFound("Nenhum dado encontrado.");
            }

            return Ok(pesagens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pesagem = await _context.Pesagem.FirstOrDefaultAsync(a => a.Id == id);

            if (pesagem == null)
            {
                return NotFound($"Pesagem com ID {id} não encontrado.");
            }

            return Ok(pesagem);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PesagemDto dto)
        {
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PesagemDto dto)
        {
            var pesagem = await _context.Pesagem.FindAsync(id);

            if (pesagem == null)
            {
                return NotFound($"Pesagem com ID {id} não encontrado.");
            }

            pesagem.Peso = dto.Peso;
            pesagem.DataPesagem = dto.DataPesagem;
            pesagem.Observacoes = dto.Observacoes;
            pesagem.AnimalId = dto.AnimalId;

            await _context.SaveChangesAsync();

            return Ok(pesagem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pesagem = await _context.Pesagem.FindAsync(id);

            if (pesagem == null)
            {
                return NotFound($"Pesagem com ID {id} não encontrado.");
            }

            _context.Pesagem.Remove(pesagem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
