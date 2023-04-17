using FirebaseAdmin.Messaging;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Log;
using UCare.Domain.Users.Events;
using UCare.Shared.Domain.ValueObjects;

namespace UCare.Web.EventsHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public class ChangeEstadoEventHandler : IDomainEventSubscriber<ChangeEstadoEvent>
    {
        /// <summary>
        /// 
        /// </summary>
        public ChangeEstadoEventHandler()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        public async Task On(ChangeEstadoEvent domainEvent)
        {
            try
            {
                // See documentation on defining a message payload.
                var message = new Message();

                message.Notification = new Notification
                {
                    Title = "Informe de cambio de estado ",
                    Body = domainEvent.EstadoNew == Estados.Activo ? "Su verificación de datos fue exitosa. Su sericio está activado" : "Su verificación de datos no fue exitosa. Su sericio está desactivado",
                };
                message.Topic = $"{domainEvent.AggregateId}";
                
                message.Data = new Dictionary<string, string>()
                {
                    { "estado", domainEvent.EstadoNew },
                };

                message.FcmOptions = new FcmOptions
                {
                    AnalyticsLabel = "estado"
                };

                message.Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        TitleLocKey = "informe_cambio_estado",
                        BodyLocKey = domainEvent.EstadoNew == Estados.Activo ? "informe_cambio_estado_activo" : "informe_cambio_estado_inactivo",
                    },
                };
                message.Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Alert = new ApsAlert
                        {
                            TitleLocKey = "informe_cambio_estado",
                            LocKey = domainEvent.EstadoNew == Estados.Activo ? "informe_cambio_estado_activo" : "informe_cambio_estado_inactivo",
                        },
                    },
                    FcmOptions = new ApnsFcmOptions
                    {
                        AnalyticsLabel = "estado"
                    }
                };

                if (domainEvent.EstadoNew == Estados.Activo)
                {
                    // Send a message to the device corresponding to the provided
                    // registration token.
                    string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                    // Response is a message ID string.
                    Console.WriteLine("ChangeEstadoEventHandler Successfully sent message: " + response);
                }
            }
            catch (Exception ex)
            {
                Log.Get().Error("Error en el evento ChangeEstadoEventHandler", ex);
            }
        }
    }
}