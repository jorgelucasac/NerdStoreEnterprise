﻿using System.Linq;
using Estudos.NSE.Carrinho.API.Model;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Estudos.NSE.Carrinho.API.Data
{
    public sealed class CarrinhoDbContext : DbContext
    {
        public CarrinhoDbContext(DbContextOptions<CarrinhoDbContext> options) : base(options)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<CarrinhoCliente> CarrinhoCliente { get; set; }
        public DbSet<CarrinhoItem> CarrinhoItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");


            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.Entity<CarrinhoCliente>().ToTable("CarrinhoCliente");
            modelBuilder.Entity<CarrinhoItem>().ToTable("CarrinhoItem");

            modelBuilder.Entity<CarrinhoCliente>()
                .HasIndex(c => c.ClienteId)
                .HasName("IDX_Cliente");

            modelBuilder.Entity<CarrinhoCliente>()
                .Ignore(c => c.Voucher)
                .OwnsOne(c => c.Voucher, v =>
                {
                    v.Property(vc => vc.Codigo)
                        .HasColumnName("VoucherCodigo")
                        .HasColumnType("varchar(50)");

                    v.Property(vc => vc.TipoDesconto)
                        .HasColumnName("TipoDesconto");

                    v.Property(vc => vc.Percentual)
                        .HasColumnName("Percentual");

                    v.Property(vc => vc.ValorDesconto)
                        .HasColumnName("ValorDesconto");
                });

            modelBuilder.Entity<CarrinhoCliente>()
                .HasMany(c => c.Itens)
                .WithOne(i => i.CarrinhoCliente)
                .HasForeignKey(c => c.CarrinhoId);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }
    }
}