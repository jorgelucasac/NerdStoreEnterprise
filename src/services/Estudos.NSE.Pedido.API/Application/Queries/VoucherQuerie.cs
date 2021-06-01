using System.Threading.Tasks;
using Estudos.NSE.Pedidos.API.Application.DTO;
using Estudos.NSE.Pedidos.Domain.Vouchers;

namespace Estudos.NSE.Pedidos.API.Application.Queries
{
    public interface IVoucherQuerie
    {
        Task<VoucherDto> ObterPorCodigo(string codigo);
    }
    public class VoucherQuerie : IVoucherQuerie
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherQuerie(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<VoucherDto> ObterPorCodigo(string codigo)
        {
            var voucher = await _voucherRepository.ObterPorCodigo(codigo);

            if (voucher is null) return null;
            if (!voucher.EstaValidoParaUtilizacao()) return null;

            return new VoucherDto
            {
                Codigo = voucher.Codigo,
                Percentual = voucher.Percentual,
                ValorDesconto = voucher.ValorDesconto,
                TipoDesconto = (int)voucher.TipoDesconto
            };
        }
    }
}