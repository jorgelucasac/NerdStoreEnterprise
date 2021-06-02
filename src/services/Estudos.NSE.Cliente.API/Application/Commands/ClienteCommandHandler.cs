using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Estudos.NSE.Clientes.API.Application.Events;
using Estudos.NSE.Clientes.API.Models;
using Estudos.NSE.Core.Messages;
using FluentValidation.Results;

namespace Estudos.NSE.Clientes.API.Application.Commands
{
    public class ClienteCommandHandler : CommandHandler,
        IRequestHandler<RegistrarClienteCommand, ValidationResult>,
        IRequestHandler<AdicionarEnderecoCommand, ValidationResult>
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
                return ValidationResult;
            }
            cliente.AdicionarEvento(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));
            _clienteRepository.Adicionar(cliente);

            return await PersistirDados(_clienteRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(AdicionarEnderecoCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var endereco = new Endereco(message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.Cep, message.Cidade, message.Estado, message.ClienteId);
            _clienteRepository.AdicionarEndereco(endereco);

            return await PersistirDados(_clienteRepository.UnitOfWork);
        }
    }
}