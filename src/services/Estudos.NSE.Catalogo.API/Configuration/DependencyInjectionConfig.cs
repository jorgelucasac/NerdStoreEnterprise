using Estudos.NSE.Catalogo.API.Data;
using Estudos.NSE.Catalogo.API.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.NSE.Catalogo.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<CatalogoDbContext>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

        }
    }
}