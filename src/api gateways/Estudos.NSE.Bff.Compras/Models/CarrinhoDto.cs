using System.Collections.Generic;

namespace Estudos.NSE.Bff.Compras.Models
{
    public class CarrinhoDto
    {
        public decimal ValorTotal { get; set; }
        public VoucherDto Voucher { get; set; }
        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; set; }
        public List<ItemCarrinhoDto> Itens { get; set; } = new List<ItemCarrinhoDto>();
    }
}