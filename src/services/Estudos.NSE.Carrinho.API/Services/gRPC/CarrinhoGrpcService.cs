using System.Threading.Tasks;
using Estudos.NSE.Carrinho.API.Data;
using Estudos.NSE.Carrinho.API.Model;
using Estudos.NSE.WebApi.Core.Usuario;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Estudos.NSE.Carrinho.API.Services.gRPC
{
    [Authorize]
    public class CarrinhoGrpcService : CarrinhoCompras.CarrinhoComprasBase
    {
        private readonly ILogger<CarrinhoGrpcService> _logger;

        private readonly IAspNetUser _user;
        private readonly ICarrinhoRepository _carrinhoRepository;


        public CarrinhoGrpcService(ILogger<CarrinhoGrpcService> logger, IAspNetUser user, ICarrinhoRepository carrinhoRepository)
        {
            _logger = logger;
            _user = user;
            _carrinhoRepository = carrinhoRepository;
        }

        public override async Task<CarrinhoClienteResponse> ObterCarrinho(ObterCarrinhoRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Chamando ObterCarrinho");

            var carrinho =  await _carrinhoRepository.ObterCarrinhoCliente(_user.ObterUserId()) ?? new CarrinhoCliente(_user.ObterUserId());

            return MapCarrinhoClienteToProtoResponse(carrinho);
        }



        private static CarrinhoClienteResponse MapCarrinhoClienteToProtoResponse(CarrinhoCliente carrinho)
        {
            var carrinhoProto = new CarrinhoClienteResponse
            {
                Id = carrinho.Id.ToString(),
                Clienteid = carrinho.ClienteId.ToString(),
                Valortotal = (double)carrinho.ValorTotal,
                Desconto = (double)carrinho.Desconto,
                Voucherutilizado = carrinho.VoucherUtilizado,
            };

            if (carrinho.Voucher != null)
            {
                carrinhoProto.Voucher = new VoucherResponse
                {
                    Codigo = carrinho.Voucher.Codigo,
                    Percentual = (double?)carrinho.Voucher.Percentual ?? 0,
                    Valordesconto = (double?)carrinho.Voucher.ValorDesconto ?? 0,
                    Tipodesconto = (int)carrinho.Voucher.TipoDesconto
                };
            }

            foreach (var item in carrinho.Itens)
            {
                carrinhoProto.Itens.Add(new CarrinhoItemResponse
                {
                    Id = item.Id.ToString(),
                    Nome = item.Nome,
                    Imagem = item.Imagem,
                    Produtoid = item.ProdutoId.ToString(),
                    Quantidade = item.Quantidade,
                    Valor = (double)item.Valor
                });
            }

            return carrinhoProto;
        }
    }
}