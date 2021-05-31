using System;
using System.Net.Http;
using System.Threading.Tasks;
using Estudos.NSE.Core.Communication;
using Estudos.NSE.WebApp.MVC.Extensions;
using Estudos.NSE.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
        private const string Api = "/api/identidade/";

        public AutenticacaoService(HttpClient httpClient, IOptions<AppSettings> appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(appSettings.Value.AutenticacaoUrl);
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin login)
        {
            var loginContent = PrepararConteudo(login);

            var response =
                await _httpClient.PostAsync($"{Api}autenticar", loginContent);

            return await TratarResponse(response);


        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro registro)
        {
            var registroContent = PrepararConteudo(registro);

            var response =
                await _httpClient.PostAsync($"{Api}nova-conta", registroContent);

            return await TratarResponse(response);
        }

        #region privates

        private async Task<UsuarioRespostaLogin> TratarResponse(HttpResponseMessage responseMessage)
        {

            if (!TratarErrosResponse(responseMessage))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(responseMessage)
                };

            }
            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(responseMessage);
        }

        #endregion
    }
}