using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Estudos.NSE.WebApp.MVC.Extensions;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public abstract class Service
    {
        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.InternalServerError:
                    throw new CustomHttpRequestException(response.StatusCode);

                case HttpStatusCode.BadRequest:// possue msg de erros na resposta
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }

        public StringContent PrepararConteudo(object dado)
        {
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
        }

        public async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }
    }
}