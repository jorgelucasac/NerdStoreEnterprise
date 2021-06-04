using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estudos.NSE.Core.Data;
using Estudos.NSE.Pagamentos.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Estudos.NSE.Pagamentos.API.Data.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly PagamentosDbContext _context;

        public PagamentoRepository(PagamentosDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void AdicionarPagamento(Models.Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
        }

        public void AdicionarTransacao(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
        }

        public async Task<Pagamento> ObterPagamentoPorPedidoId(Guid pedidoId)
        {
            return await _context.Pagamentos.AsNoTracking()
                .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
        }

        public async Task<IEnumerable<Transacao>> ObterTransacaoesPorPedidoId(Guid pedidoId)
        {
            return await _context.Transacoes.AsNoTracking()
                .Where(t => t.Pagamento.PedidoId == pedidoId).ToListAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}