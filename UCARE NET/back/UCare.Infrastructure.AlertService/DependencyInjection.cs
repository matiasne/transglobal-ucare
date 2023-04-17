using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UCare.Domain.Alertas.Events;
using UCare.Infrastructure.AlertService.Handlers;
using UCare.Shared.Infrastructure.AlertasService;

namespace UCare.Infrastructure.AlertService
{
    public static class DependencyInjection
    {
        public static void AddAlertService<T>(this IServiceCollection service, IConfiguration configuration) where T : Hub
        {
            service.AddSingleton<IAlertasService, AlertasService<T>>();

            //Eventos
            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<AlertaChangePositionEvent>, AlertaChangePositionEventHandler>();
        }
    }
}
