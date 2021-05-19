using System.Threading.Tasks;
using Estudos.NSE.WebApp.MVC.Models;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<UsuarioRespostaLogin> Login(UsuarioLogin login);
        Task<UsuarioRespostaLogin> Registro(UsuarioRegistro registro);
    }
}