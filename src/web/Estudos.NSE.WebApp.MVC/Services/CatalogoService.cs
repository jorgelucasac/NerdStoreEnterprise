using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Estudos.NSE.WebApi.Core.Usuario;
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
        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            var response = await _httpClient.GetAsync($"{Api}produtos");

            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<IEnumerable<ProdutoViewModel>>(response);
        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            var response = await _httpClient.GetAsync($"{Api}produtos/{id}");

            TratarErrosResponse(response);
            
            return await DeserializarObjetoResponse<ProdutoViewModel>(response);
        }
    }
}