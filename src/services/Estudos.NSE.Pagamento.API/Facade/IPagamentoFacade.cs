using System.Threading.Tasks;
using Estudos.NSE.Pagamentos.API.Models;

namespace Estudos.NSE.Pagamentos.API.Facade
{
    public interface IPagamentoFacade
    {
        Task<Transacao> AutorizarPagamento(Pagamento pagamento);
    }
}