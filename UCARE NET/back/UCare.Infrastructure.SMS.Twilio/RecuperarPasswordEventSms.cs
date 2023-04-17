using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Core.service;
using GQ.Log;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using UCare.Domain.Users;
using UCare.Domain.Users.Events;

namespace UCare.Infrastructure.SMS.Twilio
{
    public class RecuperarPasswordEventSms : IDomainEventSubscriber<RecuperarPasswordEvent>
    {
        private readonly IUsuarioAfiliadoRepository repository;
        public RecuperarPasswordEventSms(IUsuarioAfiliadoRepository repository)
        {
            this.repository = repository;
        }

        public async Task On(RecuperarPasswordEvent domainEvent)
        {
            try
            {
                if (domainEvent.Rol == UCare.Shared.Domain.ValueObjects.Roles.Afiliado)
                {
                    var user = await repository.GetById(domainEvent.AggregateId.Split("|")[0]);
                    if (user != null)
                    {
                        var signarure = domainEvent.Signature ?? ""; // ServicesContainer.Configuration["SMS:signature"] ?? "4D+tcThqXG5";
                        var texto = ServicesContainer.Configuration["SMS:VerificacionTelefono"];
                        texto = string.IsNullOrWhiteSpace(texto) ? "Código de Verificación {1}" : texto;
                        texto = string.Format(texto, domainEvent.Nombre, domainEvent.Code) + $" {signarure}";

                        var result = await MessageResource.CreateAsync(to: new PhoneNumber(user.Celular), from: new PhoneNumber(ServicesContainer.Configuration["SMS:Twilio:From"]), body: texto);

                        Log.Get().Warn($"Envio SMS status : {result.Status} Message :{result.ErrorMessage} ");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Get().Error("Error en el envio de sms", ex);
            }
        }
    }
}