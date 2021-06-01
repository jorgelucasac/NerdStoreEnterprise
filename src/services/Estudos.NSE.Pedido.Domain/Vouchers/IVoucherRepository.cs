using System.Threading.Tasks;
using Estudos.NSE.Core.Data;

namespace Estudos.NSE.Pedidos.Domain.Vouchers
{
    public interface IVoucherRepository:IRepository<Voucher>
    {
        Task<Voucher> ObterPorCodigo(string codigo);
    }
}