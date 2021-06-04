using System.Collections.Generic;

namespace Estudos.NSE.Catalogo.API.Models
{
    public class PagedResult<T> where T : class
    {
        /// <summary>
        /// Lista paginada
        /// </summary>
        public IEnumerable<T> List { get; set; }
        /// <summary>
        /// quantidade de itens no banco
        /// </summary>
        public int TotalResults { get; set; }
        /// <summary>
        /// pagina atual
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// quantidade de itens por página
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// paramêtro de pesquisa
        /// </summary>
        public string Query { get; set; }
    }
}