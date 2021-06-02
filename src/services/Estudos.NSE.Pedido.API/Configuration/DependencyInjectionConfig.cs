using Estudos.NSE.Core.Mediator;
using Estudos.NSE.Pedidos.API.Application.Commands;
using Estudos.NSE.Pedidos.API.Application.Events;
using Estudos.NSE.Pedidos.API.Application.Queries;
using Estudos.NSE.Pedidos.Domain.Pedidos;
using Estudos.NSE.Pedidos.Domain.Vouchers;
using Estudos.NSE.Pedidos.Infra.Data;
using Estudos.NSE.Pedidos.Infra.Data.Repository;
using Estudos.NSE.WebApi.Core.Usuario;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.NSE.Pedidos.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // API
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            // Commands
            services.AddScoped<IRequestHandler<AdicionarPedidoCommand, ValidationResult>, PedidoCommandHandler>();

            // Events
            services.AddScoped<INotificationHandler<PedidoRealizadoEvent>, PedidoEventHandler>();

            // Application
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IVoucherQuerie, VoucherQuerie>();
            services.AddScoped<IPedidoQueries, PedidoQueries>();

            // Data
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<PedidosDbContext>();

        }
    }
}