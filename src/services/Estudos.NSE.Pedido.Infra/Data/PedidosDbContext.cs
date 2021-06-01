﻿using System.Linq;
using System.Threading.Tasks;
using Estudos.NSE.Core.Data;
using Estudos.NSE.Core.DomainObjects;
using Estudos.NSE.Core.Mediator;
using Estudos.NSE.Core.Messages;
using Estudos.NSE.Pedidos.Domain.Vouchers;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Estudos.NSE.Pedidos.Infra.Data
{
    public class PedidosDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;
        public PedidosDbContext(DbContextOptions<PedidosDbContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Voucher> Vouchers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<ValidationResult>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidosDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            var sucesso = await SaveChangesAsync() > 0;
            if (sucesso)
                await _mediatorHandler.PublicarEventos(this);
            return sucesso;
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublicarEventos<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.LimparEventos());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublicarEvento(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}

