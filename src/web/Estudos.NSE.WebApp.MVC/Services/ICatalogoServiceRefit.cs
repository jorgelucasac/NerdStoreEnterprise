using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.NSE.WebApp.MVC.Models;
using Refit;

namespace Estudos.NSE.WebApp.MVC.Services
{
    public interface ICatalogoServiceRefit
    {
        [Get("/api/catalogo/produtos")]
        Task<IEnumerable<ProdutoViewModel>> ObterTodos();

        [Get("/api/catalogo/produtos/{id}")]
        Task<ProdutoViewModel> ObterPorId(Guid id);
    }
}