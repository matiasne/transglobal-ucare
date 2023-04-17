using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UCare.Domain.Users.Events;

namespace UCare.Infrastructure.SendGrid.Twilio
{
    public static class DependencyInjection
    {
        public static void AddSendGrid(this IServiceCollection service)
        {
            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<RecuperarPasswordEvent>, RecuperarPasswordEventSendGrid>();

        }
    }
}
