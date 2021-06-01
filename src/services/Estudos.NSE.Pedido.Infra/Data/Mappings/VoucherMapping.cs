using Estudos.NSE.Pedidos.Domain.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estudos.NSE.Pedidos.Infra.Data.Mappings
{
    public class VoucherMapping : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Codigo)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(a => a.Percentual)
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.ValorDesconto)
                .HasColumnType("decimal(18,2)");

            builder.ToTable("Voucher");
        }
    }
}