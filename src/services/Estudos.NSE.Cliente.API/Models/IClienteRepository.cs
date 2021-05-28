using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.NSE.Core.Data;

namespace Estudos.NSE.Clientes.API.Models
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        void Adicionar(Cliente cliente);

        Task<IEnumerable<Cliente>> ObterTodos();
        Task<Cliente> ObterPorCpf(string cpf);
        Task<bool> CpfJaUtilizado(string cpf);
    }
}