﻿using System.Linq;
using System.Threading.Tasks;
using Estudos.NSE.Core.Data;
using Estudos.NSE.Core.Messages;
using Estudos.NSE.Pagamentos.API.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Estudos.NSE.Pagamentos.API.Data
{
    public sealed class PagamentosDbContext : DbContext, IUnitOfWork
    {
        public PagamentosDbContext(DbContextOptions<PagamentosDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentosDbContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            return await SaveChangesAsync() > 0;
        }
    }
}