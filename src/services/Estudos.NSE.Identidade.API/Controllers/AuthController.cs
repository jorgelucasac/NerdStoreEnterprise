using System.Threading.Tasks;
using Estudos.NSE.Identidade.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.Identidade.API.Controllers
{
    [Route("api/identidade")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registrar(UsuarioRegistro registro)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = new IdentityUser
            {
                UserName = registro.Email,
                Email = registro.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registro.Senha);

            if (!result.Succeeded) return BadRequest();

            await _signInManager.SignInAsync(user, false);
            return Ok();

        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Login(UsuarioLogin login)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Senha, false, true);

            if (result.Succeeded) return Ok();

            return BadRequest();

        }
    }
}