using System;
using System.Collections.Generic;
using System.Linq;

namespace Estudos.NSE.Carrinho.API.Model
{
    public class CarrinhoCliente
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();

        public CarrinhoCliente(Guid clienteId)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
        }

        public CarrinhoCliente() { }


        internal void CalcularValorCarrinho()
        {
            ValorTotal = Itens.Sum(i => i.CalcularValorTotal());
        }

        internal bool CarrinhoItemExiste(CarrinhoItem item)
        {
            return Itens.Any(a => a.Id == item.Id);
        }

        internal CarrinhoItem ObterProdutoPorId(Guid id)
        {
            return Itens.First(a => a.Id == id);
        }


        internal void AdicionarItem(CarrinhoItem item)
        {
            if (!item.EhValido()) return;

            item.AssociarCarrinho(Id);

            if (CarrinhoItemExiste(item))
            {
                var itemExistente = ObterProdutoPorId(item.Id);
                itemExistente.AdicionarUnidades(item.Quantidade);

                Itens.Remove(itemExistente);
                item = itemExistente;
            }

            Itens.Add(item);
            CalcularValorCarrinho();
        }

        internal void AtualizarItem(CarrinhoItem item)
        {
            if (!item.EhValido()) return;
            item.AssociarCarrinho(Id);

            var itemExistente = ObterProdutoPorId(item.Id);

            Itens.Remove(itemExistente);
            Itens.Add(item);

            CalcularValorCarrinho();
        }

        internal void AtualizarUnidades(CarrinhoItem item, int unidades)
        {
            item.AtualizarUnidades(unidades);
            AtualizarItem(item);
        }

        internal void RemoverItem(CarrinhoItem item)
        {
            var itemExistente = ObterProdutoPorId(item.Id);
            Itens.Remove(itemExistente);
            CalcularValorCarrinho();
        }
    }
}