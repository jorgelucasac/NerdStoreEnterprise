using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Estudos.NSE.Carrinho.API.Model
{
    public interface ICarrinhoRepository : IDisposable
    {
        Task<CarrinhoCliente> ObterCarrinhoCliente(Guid clienteId);
        Task<CarrinhoItem> ObterCarrinhoItem(Guid carrinhoId, Guid produtoId);
        Task Adicionar(CarrinhoCliente carrinhoCliente);
        Task AdicionarItem(CarrinhoItem carrinhoItem);


        void Atualizar(CarrinhoCliente carrinhoCliente);
        void AtualizarItem(CarrinhoItem carrinhoItem);

        void Remover(CarrinhoCliente carrinho);
        void RemoverItem(CarrinhoItem carrinhoItem);
        Task RemoverItens(IList<CarrinhoItem> itens);

        Task<int> ObterQuantidadeItensCarrinho(Guid clienteId);
        Task<bool> PossuiCarrinho(Guid clienteId);

        Task<bool> SaveChangesAsync();
    }
}