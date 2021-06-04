using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
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

        

        public async Task<PagedResult<Produto>> ObterTodos(int pageSize, int pageIndex, string query = null)
        {
            var qtdRegistrosPular = pageSize * (pageIndex - 1); //reduz um pq as págins no banco começam de 0
            var sql = @$"SELECT * FROM Produto 
                      WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%') 
                      ORDER BY [Nome] 
                      OFFSET {qtdRegistrosPular} ROWS 
                      FETCH NEXT {pageSize} ROWS ONLY; 
                      SELECT COUNT(Id) FROM Produto 
                      WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%')";


            var multi = await _context.Database.GetDbConnection()
                .QueryMultipleAsync(sql, new { Nome = query });

            var produtos = multi.Read<Produto>();
            var total = multi.Read<int>().FirstOrDefault();

            return new PagedResult<Produto>()
            {
                List = produtos,
                TotalResults = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            };
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

        public async Task<List<Produto>> ObterProdutosPorId(string ids)
        {
            var idsGuid = ids.Split(',')
                .Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

            if (!idsGuid.All(nid => nid.Ok)) return new List<Produto>();

            var idsValue = idsGuid.Select(id => id.Value);

            return await _context.Produto.AsNoTracking()
                .Where(p => idsValue.Contains(p.Id) && p.Ativo).ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }


    }

}