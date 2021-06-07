using System;
using Estudos.NSE.Bff.Compras.Services.gRPC;
using Estudos.NSE.Carrinho.API.Services.gRPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.NSE.Bff.Compras.Configuration
{
    public static class GrpcConfig
    {
        public static void ConfigureGrpcServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GrpcServiceInterceptor>();

            services.AddScoped<ICarrinhoGrpcService, CarrinhoGrpcService>();

            services.AddGrpcClient<CarrinhoCompras.CarrinhoComprasClient>(options =>
            {
                options.Address = new Uri(configuration.GetSection("AppServicesSettings")["CarrinhoUrl"]);
            }).AddInterceptor<GrpcServiceInterceptor>();
        }
    }
}