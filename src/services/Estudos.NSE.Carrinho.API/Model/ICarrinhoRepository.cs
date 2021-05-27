using System;
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

        void RemoverItem(CarrinhoItem carrinhoItem);

        Task<bool> SaveChangesAsync();
    }
}