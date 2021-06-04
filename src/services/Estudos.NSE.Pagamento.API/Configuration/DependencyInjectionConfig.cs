using Estudos.NSE.Core.Mediator;
using Estudos.NSE.Pagamentos.API.Data;
using Estudos.NSE.Pagamentos.API.Facade;
using Estudos.NSE.Pagamentos.API.Models;
using Estudos.NSE.Pagamentos.API.Services;
using Estudos.NSE.WebApi.Core.Usuario;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.NSE.Pagamentos.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoFacade, IPagamentoFacade>();


            // Data
            services.AddScoped<IPagamentoRepository, IPagamentoRepository>();
            services.AddScoped<PagamentosDbContext>();

        }
    }
}