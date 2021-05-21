using System.Threading.Tasks;

namespace Estudos.NSE.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}