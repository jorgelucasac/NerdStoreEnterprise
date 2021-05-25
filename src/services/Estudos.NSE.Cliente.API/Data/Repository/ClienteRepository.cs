using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estudos.NSE.Clientes.API.Models;
using Estudos.NSE.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Estudos.NSE.Clientes.API.Data.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClienteDbContext _context;

        public ClienteRepository(ClienteDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Cliente>> ObterTodos()
        {
            return await _context.Clientes.AsNoTracking().ToListAsync();
        }

        public async Task<Cliente> ObterPorCpf(string cpf)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf.Numero == cpf);
        }

        public async Task<bool> CpfJaUtilizado(string cpf)
        {
            return await _context.Clientes.AnyAsync(c => c.Cpf.Numero == cpf);
        }

        public void Adicionar(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}