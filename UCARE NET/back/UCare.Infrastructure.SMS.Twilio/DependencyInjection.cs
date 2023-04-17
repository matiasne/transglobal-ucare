using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Twilio;
using UCare.Domain.Alertas.Events;
using UCare.Domain.Users.Events;
using UCare.Shared.Infrastructure.PhoneNumber;

namespace UCare.Infrastructure.SMS.Twilio
{
    public static class DependencyInjection
    {
        public static void AddSMS(this IServiceCollection service, IConfiguration configuration)
        {
            TwilioClient.Init(configuration["SMS:Twilio:Sid"], configuration["SMS:Twilio:Token"]);

            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<ChangeTelefonoEvent>, ChangeTelefonoEventSms>();
            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<RecuperarPasswordEvent>, RecuperarPasswordEventSms>();
            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<AlertaAvisarContactosEvent>, AlertaAvisarContactosEventHandler>();

            service.AddTransient<IValidatePhoneNumber, ValidatePhoneNumber>();
        }
    }
}
