using System.Net.Http;
using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Estudos.NSE.WebApp.MVC.Models;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public AutenticacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin login)
        {
            var loginContent = new StringContent(
                JsonSerializer.Serialize(login),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var response =
                await _httpClient.PostAsync("https://localhost:44359/api/identidade/autenticar", loginContent);

            var conteudoResposta = await response.Content.ReadAsStringAsync();
            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult =  JsonSerializer.Deserialize<ResponseResult>(conteudoResposta, _serializerOptions)
                };

            }
            return JsonSerializer.Deserialize<UsuarioRespostaLogin>(conteudoResposta, _serializerOptions);

          
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro registro)
        {
            var registroContent = new StringContent(
                JsonSerializer.Serialize(registro),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

            var response =
                await _httpClient.PostAsync("https://localhost:44359/api/identidade/nova-conta", registroContent);

            var conteudoResposta = await response.Content.ReadAsStringAsync();

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(conteudoResposta, _serializerOptions)
                };

            }

            var retorno = JsonSerializer.Deserialize<UsuarioRespostaLogin>(conteudoResposta, _serializerOptions);
            return retorno;
        }
    }
}