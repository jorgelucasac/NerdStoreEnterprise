using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Estudos.NSE.WebApp.MVC.Extensions;
using Estudos.NSE.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public class CatalogoService : Service, ICatalogoService
    {
        private readonly HttpClient _httpClient;
        private const string Api = "/api/catalogo/";

        public CatalogoService(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Value.CatalogoUrl);
           
        }
        public async Task<PagedViewModel<ProdutoViewModel>> ObterTodos(int pageSize, int pageIndex, string query = null)
        {
            var response = await _httpClient.GetAsync($"{Api}produtos?ps={pageSize}&page={pageIndex}&q={query}");

            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<PagedViewModel<ProdutoViewModel>>(response);
        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"{Api}produtos/{id}");

            TratarErrosResponse(response);
            
            return await DeserializarObjetoResponse<ProdutoViewModel>(response);
        }
    }
}