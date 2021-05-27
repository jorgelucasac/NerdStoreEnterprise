using System;
using System.Threading.Tasks;

namespace Estudos.NSE.Carrinho.API.Model
{
    public interface ICarrinhoRepository : IDisposable
    {
        Task<CarrinhoCliente> ObterCarrionhoCliente(Guid clienteId);
        Task Adicionar(CarrinhoCliente carrinhoCliente);
        Task AdicionarItem(CarrinhoItem carrinhoItem);


        void Atualizar(CarrinhoCliente carrinhoCliente);
        void AtualizarItem(CarrinhoItem carrinhoItem);

        Task<bool> SaveChangesAsync();
    }
}