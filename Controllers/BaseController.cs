using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Pet.Wise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected int GetIdUsuarioLogado()
        {
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int usuarioId);

            return usuarioId;
        }
    }
}
