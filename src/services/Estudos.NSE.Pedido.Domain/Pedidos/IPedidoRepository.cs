using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.NSE.Core.Data;

namespace Estudos.NSE.Pedidos.Domain.Pedidos
{
    public interface IPedidoRepository: IRepository<Pedido>
    {
        Task<Pedido> ObterPorId(Guid id);
        Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId);
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);


        /* Pedido Item */
        Task<PedidoItem> ObterItemPorId(Guid id);
        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);
    }
}