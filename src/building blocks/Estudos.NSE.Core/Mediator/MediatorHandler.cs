using System.Threading.Tasks;
using Estudos.NSE.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace Estudos.NSE.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublicarEvento<TEvent>(TEvent evento) where TEvent : Event
        {
            await _mediator.Publish(evento);
        }

        public async Task<ValidationResult> PublicarComando<TComando>(TComando comando) where TComando : Command
        {
            return await _mediator.Send(comando);
        }
    }
}