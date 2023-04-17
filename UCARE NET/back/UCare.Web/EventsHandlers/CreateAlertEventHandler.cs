using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Log;
using Microsoft.AspNetCore.SignalR;
using UCare.Application.Mapa;
using UCare.Domain.Alertas.Events;
using UCare.Shared.Infrastructure.AlertasService;
using UCare.Web.Hubs;

namespace UCare.Web.EventsHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateAlertEventHandler : IDomainEventSubscriber<AlertaCreateEvent>
    {
        private readonly IAlertasService alertasService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alertasService"></param>
        public CreateAlertEventHandler(IAlertasService alertasService)
        {
            this.alertasService = alertasService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        public async Task On(AlertaCreateEvent domainEvent)
        {
            try
            {
                await alertasService.AddAlerta(domainEvent.AggregateId);
            }
            catch (Exception ex)
            {
                Log.Get().Error("Error en el evento CreateAlertEventHandler", ex);
            }

        }
    }
}