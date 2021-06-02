using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Estudos.NSE.Core.Communication;
using Estudos.NSE.WebApp.MVC.Extensions;
using Estudos.NSE.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public class ComprasBffService : Service, IComprasBffService
    {
        private readonly HttpClient _httpClient;
        private const string Api = "/api/compras/";

        public ComprasBffService(HttpClient httpClient, IOptions<AppSettings> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.ComprasBffUrl);
        }

        #region Carrinho

        public async Task<CarrinhoViewModel> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync($"{Api}carrinho");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CarrinhoViewModel>(response);
        }

        public async Task<int> ObterQuantidadeItensCarrinho()
        {
            var response = await _httpClient.GetAsync($"{Api}carrinho-quantidade");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<int>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho)
        {
            var produtoContent = PrepararConteudo(carrinho);
            var response = await _httpClient.PostAsync($"{Api}carrinho/items", produtoContent);

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel carrinho)
        {
            var produtoContent = PrepararConteudo(carrinho);
            var response = await _httpClient.PutAsync($"{Api}carrinho/items/{produtoId}", produtoContent);

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"{Api}carrinho/items/{produtoId}");

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AplicarVoucherCarrinho(string voucherCodigo)
        {
            var voucherCodigoContent = PrepararConteudo(voucherCodigo);
            var response = await _httpClient.PostAsync($"{Api}carrinho/aplicar-voucher", voucherCodigoContent);

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        #endregion

        #region Pedido

        public PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco)
        {
            var pedido = new PedidoTransacaoViewModel
            {
                ValorTotal = carrinho.ValorTotal,
                Itens = carrinho.Itens,
                Desconto = carrinho.Desconto,
                VoucherUtilizado = carrinho.VoucherUtilizado,
                VoucherCodigo = carrinho.Voucher?.Codigo
            };

            if (endereco != null)
            {
                pedido.Endereco = new EnderecoViewModel
                {
                    Logradouro = endereco.Logradouro,
                    Numero = endereco.Numero,
                    Bairro = endereco.Bairro,
                    Cep = endereco.Cep,
                    Complemento = endereco.Complemento,
                    Cidade = endereco.Cidade,
                    Estado = endereco.Estado
                };
            }

            return pedido;
        }

        public async Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
        {
            var pedidoContent = PrepararConteudo(pedidoTransacao);

            var response = await _httpClient.PostAsync($"{Api}pedido", pedidoContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PedidoViewModel> ObterUltimoPedido()
        {
            var response = await _httpClient.GetAsync($"{Api}pedido/ultimo");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PedidoViewModel>(response);
        }

        public async Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId()
        {
            var response = await _httpClient.GetAsync($"{Api}pedido/lista-cliente/");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<PedidoViewModel>>(response);
        }

        #endregion

    }
}