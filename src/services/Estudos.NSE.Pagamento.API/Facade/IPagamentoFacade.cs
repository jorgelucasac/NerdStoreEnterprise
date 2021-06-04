using System;
using System.Threading.Tasks;
using Estudos.NSE.Core.Messages.Integrations;
using Estudos.NSE.Pagamentos.API.Models;

namespace Estudos.NSE.Pagamentos.API.Facade
{
    public interface IPagamentoFacade
    {
        Task<Transacao> AutorizarPagamento(Pagamento pagamento);
        Task<Transacao> CapturarPagamento(Transacao transacao);
        Task<Transacao> CancelarAutorizacao(Transacao transacao);
    }
}