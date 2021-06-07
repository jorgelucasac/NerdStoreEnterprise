using System;
using System.Threading.Tasks;
using Estudos.NSE.Bff.Compras.Models;
using Estudos.NSE.Carrinho.API.Services.gRPC;

namespace Estudos.NSE.Bff.Compras.Services.gRPC
{
    public interface ICarrinhoGrpcService
    {
        Task<CarrinhoDto> ObterCarrinho();
    }

    public class CarrinhoGrpcService : ICarrinhoGrpcService
    {
        private readonly CarrinhoCompras.CarrinhoComprasClient _carrinhoComprasClient;

        public CarrinhoGrpcService(CarrinhoCompras.CarrinhoComprasClient carrinhoComprasClient)
        {
            _carrinhoComprasClient = carrinhoComprasClient;
        }

        public async Task<CarrinhoDto> ObterCarrinho()
        {
            var response = await _carrinhoComprasClient.ObterCarrinhoAsync(new ObterCarrinhoRequest());

            return MapCarrinhoClienteProtoResponseToDto(response);

        }

        private static CarrinhoDto MapCarrinhoClienteProtoResponseToDto(CarrinhoClienteResponse carrinhoResponse)
        {
            var carrinhoDto = new CarrinhoDto
            {
                ValorTotal = (decimal)carrinhoResponse.Valortotal,
                Desconto = (decimal)carrinhoResponse.Desconto,
                VoucherUtilizado = carrinhoResponse.Voucherutilizado
            };

            if (carrinhoResponse.Voucher != null)
            {
                carrinhoDto.Voucher = new VoucherDto
                {
                    Codigo = carrinhoResponse.Voucher.Codigo,
                    Percentual = (decimal?)carrinhoResponse.Voucher.Percentual,
                    ValorDesconto = (decimal?)carrinhoResponse.Voucher.Valordesconto,
                    TipoDesconto = carrinhoResponse.Voucher.Tipodesconto
                };
            }

            foreach (var item in carrinhoResponse.Itens)
            {
                carrinhoDto.Itens.Add(new ItemCarrinhoDto
                {
                    Nome = item.Nome,
                    Imagem = item.Imagem,
                    ProdutoId = Guid.Parse(item.Produtoid),
                    Quantidade = item.Quantidade,
                    Valor = (decimal)item.Valor
                });
            }

            return carrinhoDto;
        }
    }
}