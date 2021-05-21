using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.NSE.Core.Data;
using Estudos.NSE.Core.DomainObjects;

namespace Estudos.NSE.Catalogo.API.Models
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterTodos();
        Task<Produto> ObterPorId(Guid id);

        void Adicionar(Produto produto);
        void Atualizar(Produto produto);
    }
}