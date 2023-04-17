using UCare.Domain.Alertas.Events;

namespace UCare.Web.EventsHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public static void AddWebEvents(this IServiceCollection service)
        {
            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<AlertaChangeStateEvent>, AlertaChangeStateEventHandler>();
            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<AlertaCreateEvent>, CreateAlertEventHandler>();
        }
    }
}
