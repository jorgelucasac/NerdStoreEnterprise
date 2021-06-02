using System.Threading.Tasks;
using Estudos.NSE.Core.Communication;
using Estudos.NSE.WebApp.MVC.Models;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public interface IClienteService
    {
        Task<EnderecoViewModel> ObterEndereco();
        Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco);
    }
}