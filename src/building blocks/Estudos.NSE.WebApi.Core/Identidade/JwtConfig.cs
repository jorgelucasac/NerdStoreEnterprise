using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.JwtExtensions;

namespace Estudos.NSE.WebApi.Core.Identidade
{
    public static class JwtConfig
    {
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            //var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //seta o tipo de altenticação 
            services.AddAuthentication(opt =>
            {
                //informa que a autenticaçação é via token
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                bearerOptions.SaveToken = true;
                //validação assimétrica do token
                bearerOptions.SetJwksOptions(new JwkOptions(appSettings.AutenticacaoJwksUrl));

                //parametros de validação simetrica do token
                //bearerOptions.TokenValidationParameters = ParametrosChaveSimetrica(null, string.Empty, string.Empty);
            });

        }

        public static void UseAuthConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        private static TokenValidationParameters ParametrosChaveSimetrica(byte[] key, string validoEm, string emissor)
        {
            return new TokenValidationParameters
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
                ValidAudience = validoEm,
                //seta o emissor
                ValidIssuer = emissor
            };
        }
    }
}