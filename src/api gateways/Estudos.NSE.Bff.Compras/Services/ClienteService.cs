using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Estudos.NSE.Bff.Compras.Extensions;
using Estudos.NSE.Bff.Compras.Models;
using Microsoft.Extensions.Options;

namespace Estudos.NSE.Bff.Compras.Services
{
    public interface IClienteService
    {
        Task<EnderecoDto> ObterEndereco();
    }

    public class ClienteService : Service, IClienteService
    {
        private readonly HttpClient _httpClient;
        private const string Api = "api/cliente/";

        public ClienteService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ClienteUrl);
        }

        public async Task<EnderecoDto> ObterEndereco()
        {
            var response = await _httpClient.GetAsync($"{Api}endereco");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<EnderecoDto>(response);
        }
    }
}