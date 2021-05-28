using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.NSE.Catalogo.API.Models;
using Estudos.NSE.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Estudos.NSE.Catalogo.API.Data
{

    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoDbContext _context;

        public ProdutoRepository(CatalogoDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Produto>> ObterTodos()
        {
            return await _context.Produto.AsNoTracking().ToListAsync();
        }

        public async Task<Produto> ObterPorId(Guid id)
        {
            return await _context.Produto.FindAsync(id);
        }

        public void Adicionar(Produto produto)
        {
            _context.Produto.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _context.Produto.Update(produto);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }


    }

}