using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Estudos.NSE.Bff.Compras.Extensions;
using Estudos.NSE.Bff.Compras.Models;
using Estudos.NSE.Core.Communication;
using Microsoft.Extensions.Options;

namespace Estudos.NSE.Bff.Compras.Services
{
    public interface IPedidoService
    {
        Task<ResponseResult> FinalizarPedido(PedidoDto pedido);
        Task<PedidoDto> ObterUltimoPedido();
        Task<IEnumerable<PedidoDto>> ObterListaPorClienteId();

        Task<VoucherDto> ObterVoucherPorCodigo(string codigo);
    }

    public class PedidoService : Service, IPedidoService
    {
        private readonly HttpClient _httpClient;
        private const string Api = "/api/";

        public PedidoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
        }

        public async Task<VoucherDto> ObterVoucherPorCodigo(string codigo)
        {
            var response = await _httpClient.GetAsync($"{Api}voucher/{codigo}");
            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VoucherDto>(response);
        }

        public async Task<ResponseResult> FinalizarPedido(PedidoDto pedido)
        {
            var pedidoContent = PrepararConteudo(pedido);

            var response = await _httpClient.PostAsync($"{Api}pedido/", pedidoContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }

        public async Task<PedidoDto> ObterUltimoPedido()
        {
            var response = await _httpClient.GetAsync($"{Api}pedido/ultimo/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PedidoDto>(response);
        }

        public async Task<IEnumerable<PedidoDto>> ObterListaPorClienteId()
        {
            var response = await _httpClient.GetAsync($"{Api}pedido/lista-cliente/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<PedidoDto>>(response);
        }

    }
}