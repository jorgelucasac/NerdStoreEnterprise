using System;
using Estudos.NSE.WebApi.Core.Usuario;
using Estudos.NSE.WebApp.MVC.Extensions;
using Estudos.NSE.WebApp.MVC.Services;
using Estudos.NSE.WebApp.MVC.Services.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Estudos.NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            #region HttpServices

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    p =>
                        p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));


            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    p =>
                        p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<ICarrinhoService, CarrinhoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.EsperarTentar())
                .AddTransientHttpErrorPolicy(
                    p =>
                        p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            #endregion
        }
    }
}