using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace Estudos.NSE.Carrinho.API.Model
{
    public class CarrinhoCliente
    {
        internal const int MaxQuantidadeItem = 5;
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();
        public ValidationResult ValidationResult { get; set; }
        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; set; }
        public Voucher Voucher { get; set; }

        public CarrinhoCliente(Guid clienteId)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
        }

        public CarrinhoCliente() { }

        public void AplicarVoucher(Voucher voucher)
        {
            Voucher = voucher;
            VoucherUtilizado = true;
            CalcularValorCarrinho();
        }

        private void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;
            var valor = ValorTotal;

            if (Voucher.TipoDesconto == TipoDescontoVoucher.Porcentagem)
            {
                if (Voucher.Percentual.HasValue)
                {
                    desconto = (valor * Voucher.Percentual.Value) / 100;
                    valor -= desconto;
                }
            }
            else
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                    valor -= desconto;
                }
            }

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }

        internal void CalcularValorCarrinho()
        {
            ValorTotal = Itens.Sum(i => i.CalcularValorTotal());
            CalcularValorTotalDesconto();
        }

        internal bool CarrinhoItemExiste(CarrinhoItem item)
        {
            return Itens.Any(a => a.ProdutoId == item.ProdutoId);
        }

        internal CarrinhoItem ObterProdutoPorId(Guid produtoId)
        {
            return Itens.First(a => a.ProdutoId == produtoId);
        }

        internal void AdicionarItem(CarrinhoItem item)
        {

            item.AssociarCarrinho(Id);

            if (CarrinhoItemExiste(item))
            {
                var itemExistente = ObterProdutoPorId(item.ProdutoId);
                itemExistente.AdicionarUnidades(item.Quantidade);

                Itens.Remove(itemExistente);
                item = itemExistente;
            }

            Itens.Add(item);
            CalcularValorCarrinho();
        }

        internal void AtualizarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);

            var itemExistente = ObterProdutoPorId(item.ProdutoId);

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
            var itemExistente = ObterProdutoPorId(item.ProdutoId);
            Itens.Remove(itemExistente);
            CalcularValorCarrinho();
        }

        internal IList<ValidationFailure> ObetErros()
        {
            return new CarrinhoClienteValidation().Validate(this).Errors;
        }

        public bool EhValido()
        {
            var erros = Itens.SelectMany(i => i.ObetErros()).ToList();
            erros.AddRange(ObetErros());
            ValidationResult = new ValidationResult(erros);

            return ValidationResult.IsValid;
        }

        public class CarrinhoClienteValidation : AbstractValidator<CarrinhoCliente>
        {
            public CarrinhoClienteValidation()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Cliente não reconhecido");

                RuleFor(c => c.Itens.Count)
                    .GreaterThan(0)
                    .WithMessage("O carrinho não possui itens");

                RuleFor(c => c.ValorTotal)
                    .GreaterThan(0)
                    .WithMessage("O valor total to carrinho precisa ser maior que 0");
            }
        }
    }
}