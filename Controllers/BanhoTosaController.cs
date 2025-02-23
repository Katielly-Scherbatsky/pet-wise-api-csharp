using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("banho-tosa")]
    public class BanhoTosaController : ControllerBase
    {
        private readonly ILogger<BanhoTosaController> _logger;
        private readonly AppDbContext _context;

        public BanhoTosaController(ILogger<BanhoTosaController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var banhosetosas = await _context.BanhoTosa.Include(v => v.Animal).ToListAsync();

            if (banhosetosas == null || banhosetosas.Count == 0)
            {
                return NotFound("Nenhum dado encontrado.");
            }

            return Ok(banhosetosas);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var banhoetosa = await _context.BanhoTosa.Include(v => v.Animal).FirstOrDefaultAsync(a => a.Id == id);

            if (banhoetosa == null)
            {
                return NotFound($"Banho e Tosa com ID {id} não encontrado.");
            }

            return Ok(banhoetosa);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BanhoTosaDto dto)
        {
            var banhoetosa = new BanhoTosaModel
            {
                Executor = dto.Executor,
                DataServico = dto.DataServico,
                Observacoes = dto.Observacoes,
                AnimalId = dto.AnimalId,
            };

            _context.BanhoTosa.Add(banhoetosa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = banhoetosa.Id }, banhoetosa);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BanhoTosaDto dto)
        {
            var banhoetosa = await _context.BanhoTosa.FindAsync(id);

            if (banhoetosa == null)
            {
                return NotFound($"Banho e Tosa com ID {id} não encontrado.");
            }

            banhoetosa.Executor = dto.Executor;
            banhoetosa.DataServico = dto.DataServico;
            banhoetosa.Observacoes = dto.Observacoes;
            banhoetosa.AnimalId = dto.AnimalId;

            await _context.SaveChangesAsync();

            return Ok(banhoetosa);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var banhoetosa = await _context.BanhoTosa.FindAsync(id);

            if (banhoetosa == null)
            {
                return NotFound($"Banho e Tosa com ID {id} não encontrado.");
            }

            _context.BanhoTosa.Remove(banhoetosa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
