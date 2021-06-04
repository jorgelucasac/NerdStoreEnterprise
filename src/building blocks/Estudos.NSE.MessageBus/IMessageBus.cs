using System;
using System.Threading.Tasks;
using EasyNetQ;
using Estudos.NSE.Core.Messages.Integrations;

namespace Estudos.NSE.MessageBus
{
    public interface IMessageBus : IDisposable
    {
        bool IsConnected { get; }
        IAdvancedBus AdvancedBus { get; }

        /// <summary>
        /// publica um evento na fila. Não aguarda retorno.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto que será enviado</typeparam>
        /// <param name="message">dado que será enviado para a fila</param>
        void Publish<T>(T message) where T : IntegrationEvent;

        /// <summary>
        /// publica um evento na fila. Não aguarda retorno.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto que será enviado</typeparam>
        /// <param name="message">dado que será enviado para a fila</param>
        Task PublishAsync<T>(T message) where T : IntegrationEvent;

        /// <summary>
        /// USADO QUANDO NÃO HÁ NECESSIDADE DE RESPONDER O EVENTO.
        /// fica ouvindo uma fila e executa uma ação quando receber a msg
        /// </summary>
        /// <typeparam name="T">Tipo de dado que será recebido da fila</typeparam>
        /// <param name="subscriptionId">Nome da Fila</param>
        /// <param name="onMessage">Dado recebido da fila</param>
        void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class;

        /// <summary>
        /// USADO QUANDO NÃO HÁ NECESSIDADE DE RESPONDER O EVENTO.
        /// fica ouvindo uma fila e executa uma ação quando receber a msg
        /// </summary>
        /// <typeparam name="T">Tipo de dado que será recebido da fila</typeparam>
        /// <param name="subscriptionId">Nome da Fila</param>
        /// <param name="onMessage">Dado recebido da fila</param>
        void SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class;

        /// <summary>
        /// publica um evento na fila e aguarda uma resposta.
        /// </summary>
        /// <typeparam name="TRequest">Tipo de objeto que será enviado</typeparam>
        /// <typeparam name="TResponse">Tipo de objeto que será retornado</typeparam>
        /// <param name="request">dado que será enviado para a fila</param>
        /// <returns></returns>
        TResponse Request<TRequest, TResponse>(TRequest request)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage;

        /// <summary>
        /// publica um evento na fila e aguarda uma resposta.
        /// </summary>
        /// <typeparam name="TRequest">Tipo de objeto que será enviado</typeparam>
        /// <typeparam name="TResponse">Tipo de objeto que será retornado</typeparam>
        /// <param name="request">dado que será enviado para a fila</param>
        /// <returns></returns>
        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage;

        /// <summary>
        /// USADO QUANDO HÁ NECESSIDADE DE RESPONDER O EVENTO.
        /// fica ouvindo uma fila e executa uma ação quando receber a msg e retorna um objeto
        /// </summary>
        /// <typeparam name="TRequest">Tipo de objeto que será recebido</typeparam>
        /// <typeparam name="TResponse">Tipo de objeto que será retornado</typeparam>
        /// <param name="responder">dado que será enviado para a fila</param>
        /// <returns></returns>
        IDisposable Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage;

        /// <summary>
        /// USADO QUANDO HÁ NECESSIDADE DE RESPONDER O EVENTO.
        /// fica ouvindo uma fila e executa uma ação quando receber a msg e retorna um objeto
        /// </summary>
        /// <typeparam name="TRequest">Tipo de objeto que será recebido</typeparam>
        /// <typeparam name="TResponse">Tipo de objeto que será retornado</typeparam>
        /// <param name="responder">dado que será enviado para a fila</param>
        /// <returns></returns>
        IDisposable RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
            where TRequest : IntegrationEvent
            where TResponse : ResponseMessage;
    }
}