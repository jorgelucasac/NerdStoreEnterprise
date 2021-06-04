using Estudos.NSE.Core.Mediator;
using Estudos.NSE.Pagamentos.API.Data;
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

            // Commands

            // Events

            // Application


            // Data
            services.AddScoped<PagamentosDbContext>();

        }
    }
}