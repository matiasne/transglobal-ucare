using FirebaseAdmin.Messaging;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Log;
using UCare.Domain.Alertas.Events;

namespace UCare.Web.EventsHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public class AlertaAlarmaSonoraEventHandler : IDomainEventSubscriber<AlertaAlarmaSonoraEvent>
    {
        /// <summary>
        /// 
        /// </summary>
        public AlertaAlarmaSonoraEventHandler()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        public async Task On(AlertaAlarmaSonoraEvent domainEvent)
        {
            try
            {
                // See documentation on defining a message payload.
                var message = new Message();

                message.Notification = new Notification
                {
                    Title = "Emergencia",
                    Body = domainEvent.Alarma ? "Activacion de alarma sonora" : "Desactivacion de alarma sonora",
                };
                message.Topic = $"{domainEvent.UsuarioId}-alarma";

                message.Data = new Dictionary<string, string>()
                {
                    { "alarma", domainEvent.Alarma.ToString() },
                };

                message.FcmOptions = new FcmOptions
                {
                    AnalyticsLabel = "Alarma"
                };

                message.Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        TitleLocKey = "emergencia_title",
                        BodyLocKey = domainEvent.Alarma ? "emergencia_body_activate" : "emergencia_body_unactive",
                        EventTimestamp = DateTime.UtcNow,
                        Priority = NotificationPriority.MAX,
                        Color = domainEvent.Alarma ? "#FF0000" : null,
                    },
                };
                message.Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Alert = new ApsAlert
                        {
                            TitleLocKey = "emergencia_title",
                            LocKey = domainEvent.Alarma ? "emergencia_body_activate" : "emergencia_body_unactive",
                        },
                        CriticalSound = new CriticalSound
                        {
                            Critical = true,
                            Name = "Alarma",
                            Volume = 1
                        }
                    },
                    FcmOptions = new ApnsFcmOptions
                    {
                        AnalyticsLabel = "Alarma"
                    }
                };

                // Send a message to the device corresponding to the provided
                // registration token.
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                // Response is a message ID string.
                Console.WriteLine("AlertaAlarmaSonoraEventHandler Successfully sent message: " + response);
            }
            catch (Exception ex)
            {
                Log.Get().Error("Error en el evento AlertaChangeStateEventHandler", ex);
            }
        }
    }
}