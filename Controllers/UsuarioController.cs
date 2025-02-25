using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly AppDbContext _context;

        public UsuarioController(ILogger<UsuarioController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _context.Usuario.ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(a => a.Id == id);

            if (usuario == null)
            {
                _logger.LogWarning("Usuário com ID {Id} não encontrado.", id);
                return NotFound($"Usuário com ID {id} não encontrado.");
            }

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsuarioDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados do usuário são obrigatórios.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = new UsuarioModel
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha,
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] UsuarioDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados do usuário são obrigatórios.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                _logger.LogWarning("Usuário com ID {Id} não encontrado.", id);
                return NotFound($"Usuário com ID {id} não encontrado.");
            }

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;
            usuario.Senha = dto.Senha;

            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                _logger.LogWarning("Usuário com ID {Id} não encontrado.", id);
                return NotFound($"Usuário com ID {id} não encontrado.");
            }

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
