using System;
using Estudos.NSE.Pedidos.Domain.Pedidos;

namespace Estudos.NSE.Pedidos.API.Application.DTO
{
    public class PedidoItemDto
    {
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string Imagem { get; set; }
        public int Quantidade { get; set; }

        public static PedidoItem ParaPedidoItem(PedidoItemDto pedidoItemDto)
        {
            return new PedidoItem(pedidoItemDto.ProdutoId, pedidoItemDto.Nome, pedidoItemDto.Quantidade,
                pedidoItemDto.Valor, pedidoItemDto.Imagem);
        }
    }
}