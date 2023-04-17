using GQ.Architecture.DDD.Domain.Bus.Event;
using UCare.Domain.Alertas.Events;
using UCare.Shared.Infrastructure.AlertasService;

namespace UCare.Infrastructure.AlertService.Handlers
{
    public class AlertaChangePositionEventHandler : IDomainEventSubscriber<AlertaChangePositionEvent>
    {
        private readonly IAlertasService alertasService;
        public AlertaChangePositionEventHandler(IAlertasService alertasService)
        {
            this.alertasService = alertasService;
        }
        public Task On(AlertaChangePositionEvent domainEvent)
        {
            alertasService.UpdatePosition(domainEvent.AggregateId, domainEvent.Lat, domainEvent.Lon);

            return Task.CompletedTask;
        }
    }
}
