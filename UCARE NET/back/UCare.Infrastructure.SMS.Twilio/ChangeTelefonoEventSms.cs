using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Core.service;
using GQ.Log;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using UCare.Domain.Users.Events;

namespace UCare.Infrastructure.SMS.Twilio
{
    public class ChangeTelefonoEventSms : IDomainEventSubscriber<ChangeTelefonoEvent>
    {
        public async Task On(ChangeTelefonoEvent domainEvent)
        {
            try
            {
                var signarure = domainEvent.Signarure ?? ""; // ServicesContainer.Configuration["SMS:signature"] ?? "4D+tcThqXG5";
                var texto = ServicesContainer.Configuration["SMS:VerificacionTelefono"];
                texto = string.IsNullOrWhiteSpace(texto) ? "Código de Verificación {1}" : texto;
                texto = string.Format(texto, domainEvent.Nombre, domainEvent.Codigo) + $" {signarure}";

                var result = await MessageResource.CreateAsync(to: new PhoneNumber(domainEvent.Telefono), from: new PhoneNumber(ServicesContainer.Configuration["SMS:Twilio:From"]), body: texto);

                Log.Get().Warn($"Envio SMS status : {result.Status} Message :{result.ErrorMessage} Texto :{texto} ");
            }
            catch (Exception ex)
            {
                Log.Get().Error("Error en el envio de sms", ex);
            }
        }
    }
}