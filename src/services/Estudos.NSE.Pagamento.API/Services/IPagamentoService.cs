using System.Threading.Tasks;
using Estudos.NSE.Core.Messages.Integrations;
using Estudos.NSE.Pagamentos.API.Models;

namespace Estudos.NSE.Pagamentos.API.Services
{
    public interface IPagamentoService
    {
        Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento);
    }
}