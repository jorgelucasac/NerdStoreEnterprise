using System;
using System.Net;
using System.Net.Http;
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
    }
}