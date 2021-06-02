using Estudos.NSE.Clientes.API.Application.Commands;
using Estudos.NSE.Clientes.API.Application.Events;
using Estudos.NSE.Clientes.API.Data;
using Estudos.NSE.Clientes.API.Data.Repository;
using Estudos.NSE.Clientes.API.Models;
using Estudos.NSE.Core.Mediator;
using Estudos.NSE.WebApi.Core.Usuario;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace Estudos.NSE.Clientes.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
            services.AddScoped<IRequestHandler<AdicionarEnderecoCommand, ValidationResult>, ClienteCommandHandler>();

            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ClienteDbContext>();
        }
    }
}