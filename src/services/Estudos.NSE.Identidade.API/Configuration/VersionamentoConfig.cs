using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Estudos.NSE.Identidade.API.Configuration
{
    public static class VersionamentoConfig
    {
        public static void AddVersionamento(this IServiceCollection services)
        {

            services.AddApiVersioning(options =>
            {
                //assume a versão default quando não especificar a versão
                options.AssumeDefaultVersionWhenUnspecified = true;
                //versão default da API
                options.DefaultApiVersion = new ApiVersion(1, 0);
                //informa no header se a API está na ultima versão ou obsoleta 
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}