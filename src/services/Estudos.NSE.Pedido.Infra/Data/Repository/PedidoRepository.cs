﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estudos.NSE.Core.Data;
using Estudos.NSE.Pedidos.Domain.Pedidos;
using Microsoft.EntityFrameworkCore;

namespace Estudos.NSE.Pedidos.Infra.Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidosDbContext _context;

        public PedidoRepository(PedidosDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;


        public async Task<Pedido> ObterPorId(Guid id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId)
        {
            return await _context.Pedidos
                .Include(p => p.PedidoItems)
                .AsNoTracking()
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }


        public void Adicionar(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
        }

        public void Atualizar(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }


        public async Task<PedidoItem> ObterItemPorId(Guid id)
        {
            return await _context.PedidoItens.FindAsync(id);
        }

        public async Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId)
        {
            return await _context.PedidoItens
                .FirstOrDefaultAsync(p => p.ProdutoId == produtoId && p.PedidoId == pedidoId);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}