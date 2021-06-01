using System;
using System.Net.Http;
using System.Threading.Tasks;
using Estudos.NSE.Bff.Compras.Extensions;
using Estudos.NSE.Bff.Compras.Models;
using Estudos.NSE.Core.Communication;
using Microsoft.Extensions.Options;

namespace Estudos.NSE.Bff.Compras.Services
{
    public interface ICarrinhoService
    {
        Task<CarrinhoDto> ObterCarrinho();
        Task<int> ObterQuantidadeCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDto produto);
        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDto carrinho);
        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
        Task<ResponseResult> AplicarVoucherCarrinho(VoucherDto voucher);
    }

    public class CarrinhoService : Service, ICarrinhoService
    {
        private readonly HttpClient _httpClient;
        private const string Api = "/api/carrinho/";

        public CarrinhoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
        }

        public async Task<CarrinhoDto> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync(Api);

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CarrinhoDto>(response);
        }

        public async Task<int> ObterQuantidadeCarrinho()
        {
            var response = await _httpClient.GetAsync($"{Api}quantidade-itens-carrinho");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<int>(response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDto produto)
        {
            var produtoContent = PrepararConteudo(produto);
            var response = await _httpClient.PostAsync(Api, produtoContent);

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDto carrinho)
        {
            var produtoContent = PrepararConteudo(carrinho);
            var response = await _httpClient.PutAsync($"{Api}{produtoId}", produtoContent);

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"{Api}{produtoId}");

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AplicarVoucherCarrinho(VoucherDto voucher)
        {
            var voucherContent = PrepararConteudo(voucher);
            var response = await _httpClient.PostAsync($"{Api}aplicar-voucher", voucherContent);

            if (!TratarErrosResponse(response))
                return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }
    }
}