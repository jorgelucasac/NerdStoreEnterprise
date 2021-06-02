using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Estudos.NSE.Core.Communication;
using Estudos.NSE.WebApp.MVC.Extensions;
using Estudos.NSE.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public class ClienteService : Service, IClienteService
    {
        private readonly HttpClient _httpClient;
        private const string Api = "/api/cliente/";

        public ClienteService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
        }

        public async Task<EnderecoViewModel> ObterEndereco()
        {
            var response = await _httpClient.GetAsync($"{Api}endereco");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<EnderecoViewModel>(response);
        }

        public async Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco)
        {
            var enderecoContent = PrepararConteudo(endereco);

            var response = await _httpClient.PostAsync($"{Api}endereco", enderecoContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return RetornoOk();
        }
    }
}