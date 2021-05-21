using System;
using Estudos.NSE.Core.DomainObjects;

namespace Estudos.NSE.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        public IUnitOfWork UnitOfWork { get; }
    }
}