using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Log;
using Microsoft.AspNetCore.SignalR;
using UCare.Application.Mapa;
using UCare.Domain.Alertas.Events;
using UCare.Web.Hubs;

namespace UCare.Web.EventsHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public class AlertaChangeStateEventHandler : IDomainEventSubscriber<AlertaChangeStateEvent>
    {
        private readonly MapaApp app;
        private readonly IHubContext<MonitorHub> hub;
        private readonly IEventBus eventBus;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="hub"></param>
        /// <param name="eventBus"></param>
        public AlertaChangeStateEventHandler(MapaApp app, IHubContext<MonitorHub> hub, IEventBus eventBus)
        {
            this.app = app;
            this.hub = hub;
            this.eventBus = eventBus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        public async Task On(AlertaChangeStateEvent domainEvent)
        {
            try
            {
                //var entity = app.GetById(domainEvent.AggregateId);
                //await hub.Clients.All.SendAsync("OnAddNewAlerta", new { Id = domainEvent.AggregateId, AfiliadoId = domainEvent.AfiliadoId, Estado = domainEvent.EstadoNuevo });
            }
            catch (Exception ex)
            {
                Log.Get().Error("Error en el evento AlertaChangeStateEventHandler", ex);
            }
        }
    }
}