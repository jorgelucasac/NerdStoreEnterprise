using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.NSE.Core.Communication;
using Estudos.NSE.WebApp.MVC.Models;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public interface IComprasBffService
    {
        Task<CarrinhoViewModel> ObterCarrinho();
        Task<int> ObterQuantidadeItensCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho);
        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel carrinho);
        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
        Task<ResponseResult> AplicarVoucherCarrinho(string voucherCodigo);
        PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco);
        Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao);
        Task<PedidoViewModel> ObterUltimoPedido();
        Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId();
    }
}