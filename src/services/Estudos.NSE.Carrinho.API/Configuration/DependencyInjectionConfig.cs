using Estudos.NSE.Carrinho.API.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.NSE.Carrinho.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<CarrinhoDbContext>();
        }
    }
}