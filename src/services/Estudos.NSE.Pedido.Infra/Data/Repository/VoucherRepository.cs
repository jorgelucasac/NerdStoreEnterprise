using System.Threading.Tasks;
using Estudos.NSE.Core.Data;
using Estudos.NSE.Pedidos.Domain.Vouchers;
using Microsoft.EntityFrameworkCore;

namespace Estudos.NSE.Pedidos.Infra.Data.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly PedidosDbContext _context;

        public VoucherRepository(PedidosDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Voucher> ObterPorCodigo(string codigo)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(v => v.Codigo == codigo);
        }

        public void Atualizar(Voucher voucher)
        {
            _context.Vouchers.Update(voucher);
        }


        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}