using Estudos.NSE.Carrinho.API.Data;
using Estudos.NSE.Carrinho.API.Data.Repository;
using Estudos.NSE.Carrinho.API.Model;
using Estudos.NSE.WebApi.Core.Usuario;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.NSE.Carrinho.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
           

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddScoped<CarrinhoDbContext>();

            services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
        }
    }
}