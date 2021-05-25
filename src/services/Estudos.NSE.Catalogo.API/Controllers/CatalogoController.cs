using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.NSE.Catalogo.API.Models;
using Estudos.NSE.WebApi.Core.Controllers;
using Estudos.NSE.WebApi.Core.Identidade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.NSE.Catalogo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CatalogoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;

        public CatalogoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [AllowAnonymous]
        [HttpGet("produtos")]
        public async Task<IEnumerable<Produto>> Index()
        {
            return await _produtoRepository.ObterTodos();
        }

        [HttpGet("produtos/{id}")]
        [ClaimsAuthorize("Catalogo", "Ler")]
        public async Task<Produto> ProdutoDetalhe(Guid id)
        {
            return await _produtoRepository.ObterPorId(id);
        }
    }
}