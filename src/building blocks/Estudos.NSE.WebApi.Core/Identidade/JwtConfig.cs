using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Estudos.NSE.WebApi.Core.Identidade
{
    public static class JwtConfig
    {
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = true;
                bearerOptions.SaveToken = true;

                //parametros de validação do token
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    //validar a assinatura do token?
                    ValidateIssuerSigningKey = true,
                    //chave de criptografia
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    //validar o emisor?
                    ValidateIssuer = true,
                    //validar os dominios onde o token é válido?
                    ValidateAudience = true,
                    //seta o doimio onde o token é valido
                    ValidAudience = appSettings.ValidoEm,
                    //seta o emissor
                    ValidIssuer = appSettings.Emissor
                };
            });

        }

        public static void UseAuthConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}