using Estudos.NSE.Identidade.API.Services;
using Estudos.NSE.WebApi.Core.Identidade;
using Estudos.NSE.WebApi.Core.Usuario;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetDevPack.Security.JwtSigningCredentials.AspNetCore;

namespace Estudos.NSE.Identidade.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfiguration(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddVersionamento();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddScoped<AuthenticationService>();

        }

        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseJwksDiscovery();
        }
    }
}