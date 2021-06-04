using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.NSE.WebApp.MVC.Models;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public interface ICatalogoService
    {
        Task<PagedViewModel<ProdutoViewModel>> ObterTodos(int pageSize, int pageIndex, string query = null);
        Task<ProdutoViewModel> ObterPorId(Guid id);
    }
}