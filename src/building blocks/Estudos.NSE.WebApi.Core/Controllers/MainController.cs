using System.Collections.Generic;
using System.Linq;
using Estudos.NSE.Core.Communication;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Estudos.NSE.WebApi.Core.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        protected ICollection<string> Erros = new List<string>();
        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                {"Mensagens", Erros.ToArray()}
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors).ToList();
            erros.ForEach(erro => AdicionarErroProcessamento(erro.ErrorMessage));

            return CustomResponse();
        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
            {
                AdicionarErroProcessamento(erro.ErrorMessage);

            }
            return CustomResponse();
        }

        protected ActionResult CustomResponse(ResponseResult result)
        {
            ResponsePossuiErros(result);
            return CustomResponse();
        }

        protected bool ResponsePossuiErros(ResponseResult result)
        {
            if (result == null) return false;

            foreach (var mensagen in result.Errors.Mensagens)
            {
                ModelState.AddModelError(string.Empty, mensagen);
            }

            return true;
        }

        protected bool OperacaoValida() => !Erros.Any();

        protected void AdicionarErroProcessamento(string erro) => Erros.Add(erro);

        protected void LimparErrosProcessamento() => Erros.Clear();
    }
}