using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;

namespace Estudos.NSE.Carrinho.API.Model
{
    public class CarrinhoItem
    {
      
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

        [JsonIgnore]
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

        internal IList<ValidationFailure> ObetErros()
        {
            return new ItenCarrinhoValidation().Validate(this).Errors;
        }

        internal bool EhValido()
        {
            return new ItenCarrinhoValidation().Validate(this).IsValid;
        }


        public class ItenCarrinhoValidation : AbstractValidator<CarrinhoItem>
        {
            public ItenCarrinhoValidation()
            {
                RuleFor(c => c.ProdutoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do produto inválido");

                RuleFor(c => c.Nome)
                    .NotEmpty()
                    .WithMessage("nome do produto não foi informado");

                RuleFor(c => c.Quantidade)
                    .GreaterThan(0)
                    .WithMessage(item => $"a quntidade miníma para o {item.Nome} é 1");

                RuleFor(c => c.Quantidade)
                    .LessThanOrEqualTo(CarrinhoCliente.MaxQuantidadeItem)
                    .WithMessage(item => $"a quantidade máxima do {item.Nome} é {CarrinhoCliente.MaxQuantidadeItem}");

                RuleFor(c => c.Valor)
                    .GreaterThan(0)
                    .WithMessage(item => $"o valor do {item.Nome} precisa ser maior que 0");

            }
        }
    }
}