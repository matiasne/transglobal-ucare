using Microsoft.Extensions.DependencyInjection;
using UCare.Domain.Users.Events;

namespace UCare.Infrastructure.SendinBlue
{
    public static class DependencyInjection
    {
        public static void SendinBlue(this IServiceCollection service)
        {
            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<RecuperarPasswordEvent>, RecuperarPasswordEventSendinBlue>();

        }
    }
}
