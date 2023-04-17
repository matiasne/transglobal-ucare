using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Core.service;
using GQ.Log;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using UCare.Domain.Alertas;
using UCare.Domain.Alertas.Events;
using UCare.Domain.Users;
using UCare.Domain.Users.Events;
using UCare.Shared.Domain.CodigoPaices;
using UCare.Shared.Infrastructure.PhoneNumber;

namespace UCare.Infrastructure.SMS.Twilio
{
    public class AlertaAvisarContactosEventHandler : IDomainEventSubscriber<AlertaAvisarContactosEvent>
    {
        private readonly IUsuarioAfiliadoRepository afiliadoRepository;
        private readonly IAlertaRepository alertaRepository;
        private readonly IValidatePhoneNumber validatePhone;
        private readonly ICodigosPaicesRepositorio codigosPaices;

        public AlertaAvisarContactosEventHandler(IUsuarioAfiliadoRepository afiliadoRepository, IAlertaRepository alertaRepository, IValidatePhoneNumber validatePhone, ICodigosPaicesRepositorio codigosPaices)
        {
            this.afiliadoRepository = afiliadoRepository;
            this.alertaRepository = alertaRepository;
            this.validatePhone = validatePhone;
            this.codigosPaices = codigosPaices;
        }
        public async Task On(AlertaAvisarContactosEvent domainEvent)
        {
            try
            {
                var afiliado = await afiliadoRepository.GetById(domainEvent.UsuarioId);
                var alerta = await alertaRepository.GetById(domainEvent.AggregateId);
                var codigos = codigosPaices.GetCodigoPaisList();

                List<Telefono> phones = new List<Telefono>();
                foreach (var item in afiliado.Contactos)
                {
                    var arr = item.Telefono.Split(';');

                    foreach (var phone in arr)
                    {
                        var codes = codigos.Where(x => phone.StartsWith(x.CountryCode));
                        foreach (var code in codes)
                        {
                            if (validatePhone.Validate(phone, code.CountryCode))
                            {
                                phones.Add(new Telefono { Codigo = code.CountryCode, Number = phone });
                            }
                            else
                            {
                                Log.Get().Warn($"Numero no valido para envio de sms : {phone}");
                            }
                        }
                        if (!codes.Any())
                        {
                            Log.Get().Warn($"Numero no valido para envio de sms : {phone}");
                        }
                    }
                }

                foreach (var phone in phones)
                {
                    var texto = ServicesContainer.Configuration["SMS:TextoEnvioDeEmergencia"];
                    texto = string.IsNullOrWhiteSpace(texto) ? "Nos comunicamos para informarle que {0} a tenido una emergencia y que ya hemos enviado el servicio de emergencias" : texto;
                    texto = string.Format(texto, afiliado.UsuarioNombre);

                    var result = await MessageResource.CreateAsync(to: new PhoneNumber(phone.Number), from: new PhoneNumber(ServicesContainer.Configuration["SMS:Twilio:From"]), body: texto);

                    Log.Get().Warn($"Envio SMS status : {result.Status} Message :{result.ErrorMessage} ");
                }
            }
            catch (Exception ex)
            {
                Log.Get().Error("Error en el envio de sms", ex);
            }
        }
    }

    public class Telefono
    {
        public string Codigo { get; set; }
        public string Number { get; set; }
    }
}