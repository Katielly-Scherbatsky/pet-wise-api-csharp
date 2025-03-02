using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pet.Wise.Api.DataContexts;
using Pet.Wise.Api.Dto;
using Pet.Wise.Api.Services;

namespace Pet.Wise.Api.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly TokenService _tokenService;
        private readonly AppDbContext _context;

        public AuthController(ILogger<AuthController> logger, TokenService tokenService, AppDbContext context)
        {
            _logger = logger;
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                var usuario = await _context.Usuario.FirstOrDefaultAsync(x => x.Email == request.Email);
                if (usuario == null)
                {
                    return Unauthorized("Usuário ou senha inválidos.");
                }

                if (request.Senha == usuario.Senha)
                {
                    var token = _tokenService.GenerateToken(usuario.Id, usuario.Email);
                    return Ok(new { token });
                }

                return Unauthorized("Usuário ou senha inválidos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu uma falha ao realizar a autenticação. Tente novamente mais tarde.");
                return BadRequest();
            }
        }
    }
}
