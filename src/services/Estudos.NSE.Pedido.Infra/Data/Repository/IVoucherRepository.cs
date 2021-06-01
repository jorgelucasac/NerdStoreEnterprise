using Estudos.NSE.Core.Data;
using Estudos.NSE.Pedidos.Domain.Vouchers;

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





        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}