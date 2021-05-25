using System.Threading.Tasks;
using Estudos.NSE.Core.Messages;
using FluentValidation.Results;

namespace Estudos.NSE.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<TEvent>(TEvent evento) where TEvent : Event;
        Task<ValidationResult> PublicarComando<TComando>(TComando comando) where TComando : Command;
    }
}