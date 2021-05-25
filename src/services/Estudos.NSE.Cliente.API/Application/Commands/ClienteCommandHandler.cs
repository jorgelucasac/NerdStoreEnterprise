using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Estudos.NSE.Clientes.API.Models;
using Estudos.NSE.Core.Messages;
using FluentValidation.Results;

namespace Estudos.NSE.Clientes.API.Application.Commands
{
    public class ClienteCommandHandler : CommandHandler, IRequestHandler<RegistrarClienteCommand, ValidationResult>
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);

            var cpfJaUtilizado = await _clienteRepository.CpfJaUtilizado(cliente.Cpf.Numero);
            if (cpfJaUtilizado)
            {
                AdicionarErro("Este cpf já está utilizado");
                return message.ValidationResult;
            }

            _clienteRepository.Adicionar(cliente);

            return await PersistirDados(_clienteRepository.UnitOfWork);
        }
    }
}