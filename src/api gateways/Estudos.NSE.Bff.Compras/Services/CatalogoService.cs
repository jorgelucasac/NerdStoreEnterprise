using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Estudos.NSE.Bff.Compras.Extensions;
using Estudos.NSE.Bff.Compras.Models;
using Microsoft.Extensions.Options;

namespace Estudos.NSE.Bff.Compras.Services
{
    public interface ICatalogoService
    {
        Task<ItemProdutoDto> ObterPorId(Guid id);
        Task<IEnumerable<ItemProdutoDto>> ObterItens(IEnumerable<Guid> @select);
    }

    public class CatalogoService : Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;
        private const string Api = "/api/catalogo/";

        public CatalogoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl);
        }

        public async Task<ItemProdutoDto> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"{Api}produtos/{id}");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<ItemProdutoDto>(response);
        }

        public async Task<IEnumerable<ItemProdutoDto>> ObterItens(IEnumerable<Guid> ids)
        {
            var idsRequest = string.Join(",", ids);

            var response = await _httpClient.GetAsync($"{Api}produtos/lista/{idsRequest}/");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<ItemProdutoDto>>(response);
        }
    }
}