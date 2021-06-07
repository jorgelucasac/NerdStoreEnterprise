using System.Threading.Tasks;
using Estudos.NSE.WebApp.MVC.Models;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<UsuarioRespostaLogin> Login(UsuarioLogin login);
        Task<UsuarioRespostaLogin> Registro(UsuarioRegistro registro);
        Task RealizarLogin(UsuarioRespostaLogin resposta);
        Task Logout();
        bool TokenExpirado();
        Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken);
        Task<bool> RefreshTokenValido();
    }
}