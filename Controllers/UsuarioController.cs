using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Models;

namespace Pet.Wise.Api.Controllers
{
    public class UsuarioController : BaseController
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly AppDbContext _context;

        public UsuarioController(ILogger<UsuarioController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuarios = await _context.Usuario.ToListAsync();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar usuários");
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var usuario = await _context.Usuario.FirstOrDefaultAsync(a => a.Id == id);

                if (usuario == null)
                {
                    _logger.LogWarning("Usuário com ID {Id} não encontrado.", id);
                    return NotFound($"Usuário com ID {id} não encontrado.");
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao buscar usuário pelo Id {Id}", id);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsuarioDto dto)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao criar usuário");
                return BadRequest();
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put(UsuarioDto dto)
        {
            var usuarioId = 0;
            try
            {
                if (dto == null)
                {
                    return BadRequest("Dados do usuário são obrigatórios.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                usuarioId = GetIdUsuarioLogado();
                var usuario = await _context.Usuario.FindAsync(usuarioId);
                if (usuario == null)
                {
                    _logger.LogWarning("Usuário com ID {Id} não encontrado.", usuarioId);
                    return NotFound($"Usuário com ID {usuarioId} não encontrado.");
                }

                usuario.Nome = dto.Nome;
                usuario.Email = dto.Email;
                usuario.Senha = dto.Senha;

                await _context.SaveChangesAsync();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao atualizar usuário com Id: {Id}", usuarioId);
                return BadRequest();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            var usuarioId = 0;
            try
            {
                usuarioId = GetIdUsuarioLogado();
                var usuario = await _context.Usuario.FindAsync(usuarioId);
                if (usuario == null)
                {
                    _logger.LogWarning("Usuário com ID {Id} não encontrado.", usuarioId);
                    return NotFound($"Usuário com ID {usuarioId} não encontrado.");
                }

                _context.Usuario.Remove(usuario);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao excluir usuário com Id: {Id}", usuarioId);
                return BadRequest();
            }
        }
    }
}
