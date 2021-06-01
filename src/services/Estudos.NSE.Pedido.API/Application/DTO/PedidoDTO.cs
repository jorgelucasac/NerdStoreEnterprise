using System;
using System.Collections.Generic;
using Estudos.NSE.Pedidos.Domain.Pedidos;

namespace Estudos.NSE.Pedidos.API.Application.DTO
{
    public class PedidoDto
    {
        public Guid Id { get; set; }
        public int Codigo { get; set; }

        public int Status { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }

        public decimal Desconto { get; set; }
        public string VoucherCodigo { get; set; }
        public bool VoucherUtilizado { get; set; }

        public List<PedidoItemDto> PedidoItems { get; set; }
        public EnderecoDto Endereco { get; set; }

        public static PedidoDto ParaPedidoDto(Pedido pedido)
        {
            var pedidoDto = new PedidoDto
            {
                Id = pedido.Id,
                Codigo = pedido.Codigo,
                Status = (int)pedido.PedidoStatus,
                Data = pedido.DataCadastro,
                ValorTotal = pedido.ValorTotal,
                Desconto = pedido.Desconto,
                VoucherUtilizado = pedido.VoucherUtilizado,
                PedidoItems = new List<PedidoItemDto>(),
                Endereco = new EnderecoDto()
            };

            foreach (var item in pedido.PedidoItems)
            {
                pedidoDto.PedidoItems.Add(new PedidoItemDto
                {
                    Nome = item.ProdutoNome,
                    Imagem = item.ProdutoImagem,
                    Quantidade = item.Quantidade,
                    ProdutoId = item.ProdutoId,
                    Valor = item.ValorUnitario,
                    PedidoId = item.PedidoId
                });
            }

            pedidoDto.Endereco = new EnderecoDto
            {
                Logradouro = pedido.Endereco.Logradouro,
                Numero = pedido.Endereco.Numero,
                Complemento = pedido.Endereco.Complemento,
                Bairro = pedido.Endereco.Bairro,
                Cep = pedido.Endereco.Cep,
                Cidade = pedido.Endereco.Cidade,
                Estado = pedido.Endereco.Estado,
            };

            return pedidoDto;
        }
    }
}