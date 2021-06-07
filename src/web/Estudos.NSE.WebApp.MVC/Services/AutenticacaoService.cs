using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Estudos.NSE.Core.Communication;
using Estudos.NSE.WebApi.Core.Usuario;
using Estudos.NSE.WebApp.MVC.Extensions;
using Estudos.NSE.WebApp.MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public class AutenticacaoService : Service, IAutenticacaoService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAspNetUser _aspNetUser;
        private const string Api = "/api/identidade/";

        public AutenticacaoService(HttpClient httpClient,
            IOptions<AppSettings> appSettings,
            IAuthenticationService authenticationService,
            IAspNetUser aspNetUser)
        {
            _httpClient = httpClient;
            _authenticationService = authenticationService;
            _aspNetUser = aspNetUser;
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

        public async Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken)
        {
            var refreshTokenContent = PrepararConteudo(refreshToken);

            var response = await _httpClient.PostAsync($"{Api}refresh-token", refreshTokenContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }
        public async Task RealizarLogin(UsuarioRespostaLogin resposta)
        {
            var token = ObterTokenFormatado(resposta.AccessToken);
            var claims = new List<Claim>
            {
                new Claim("JWT", resposta.AccessToken),
                new Claim("RefreshToken", resposta.RefreshToken)
            };

            claims.AddRange(token.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                IsPersistent = true //vai durar vários requests
            };

            await _authenticationService.SignInAsync(
                _aspNetUser.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        }
        public Task Logout()
        {
            return _authenticationService.SignOutAsync(
                _aspNetUser.ObterHttpContext(),
                CookieAuthenticationDefaults.AuthenticationScheme,
                null);
        }

        public bool TokenExpirado()
        {
            var jwt = _aspNetUser.ObterUserToken();
            if (jwt is null) return false;

            var token = ObterTokenFormatado(jwt);
            return token.ValidTo.ToLocalTime() < DateTime.Now;
        }

        public async Task<bool> RefreshTokenValido()
        {
            var resposta = await UtilizarRefreshToken(_aspNetUser.ObterUserRefreshToken());

            if (resposta.AccessToken != null && resposta.ResponseResult == null)
            {
                await RealizarLogin(resposta);
                return true;
            }

            return false;
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

        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
        }

        #endregion
    }
}