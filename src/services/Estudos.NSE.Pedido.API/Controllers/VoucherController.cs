using System.Threading.Tasks;
using Estudos.NSE.Pedidos.API.Application.DTO;
using Estudos.NSE.Pedidos.API.Application.Queries;
using Estudos.NSE.WebApi.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.Pedidos.API.Controllers
{
    [Authorize, Route("api/[controller]")]
    public class VoucherController : MainController
    {
        private readonly IVoucherQuerie _voucherQuerie;

        public VoucherController(IVoucherQuerie voucherQuerie)
        {
            _voucherQuerie = voucherQuerie;
        }

        [HttpGet("{codigo}")]
        [ProducesResponseType(typeof(VoucherDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorCodigo(string codigo)
        {
            if (string.IsNullOrEmpty(codigo)) return NotFound();

            var voucher = await _voucherQuerie.ObterPorCodigo(codigo);

            return voucher is null ? NotFound() : CustomResponse(voucher);
        }
    }
}