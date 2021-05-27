using System;
using FluentValidation;

namespace Estudos.NSE.Carrinho.API.Model
{
    public class CarrinhoItem
    {
        internal const int MaxQuantidadeItem = 5;
        public CarrinhoItem()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string Imagem { get; set; }
        public Guid CarrinhoId { get; set; }

        public CarrinhoCliente CarrinhoCliente { get; set; }

        internal void AssociarCarrinho(Guid carrinhoId)
        {
            CarrinhoId = carrinhoId;
        }

        internal decimal CalcularValorTotal()
        {
            return Valor * Quantidade;
        }

        internal int AdicionarUnidades(int unidades)
        {
            return Quantidade += unidades;
        }

        internal int AtualizarUnidades(int unidades)
        {
            return Quantidade = unidades;
        }

        internal bool EhValido()
        {
            return new ItenPedidoValidation().Validate(this).IsValid;
        }


        public class ItenPedidoValidation: AbstractValidator<CarrinhoItem>
        {
            public ItenPedidoValidation()
            {
                RuleFor(c => c.ProdutoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do produto inválido");

                RuleFor(c => c.Nome)
                    .NotEmpty()
                    .WithMessage("nome do produto não foi informado");

                RuleFor(c => c.Quantidade)
                    .GreaterThan(0)
                    .WithMessage("a quntidade miníma de um item é 1");

                RuleFor(c => c.Quantidade)
                    .LessThan(MaxQuantidadeItem)
                    .WithMessage($"a quntidade máxima de um item é {MaxQuantidadeItem}");

                RuleFor(c => c.Valor)
                    .GreaterThan(0)
                    .WithMessage("o valor do produto precisa ser maior que 0");

            }
        }
    }
}