using FirebaseAdmin.Messaging;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Log;
using UCare.Domain.Alertas.Events;

namespace UCare.Web.EventsHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public class AlertaFinalizarEventHandler : IDomainEventSubscriber<AlertaFinalizarEvent>
    {
        /// <summary>
        /// 
        /// </summary>
        public AlertaFinalizarEventHandler()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        public async Task On(AlertaFinalizarEvent domainEvent)
        {
            try
            {
                // See documentation on defining a message payload.
                var message = new Message();

                message.Notification = new Notification
                {
                    Title = "Asistencia finalizada",
                    Body = "",
                };
                message.Topic = $"{domainEvent.UsuarioId}";

                message.Data = new Dictionary<string, string>()
                {
                    { "asistencia", "false" },
                };

                message.FcmOptions = new FcmOptions
                {
                    AnalyticsLabel = "Finalizada"
                };

                message.Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        TitleLocKey = "asistencia_finalizada_title",
                        BodyLocKey = "asistencia_finalizada_body",
                        EventTimestamp = DateTime.UtcNow,
                        Priority = NotificationPriority.MAX,
                    },
                    FcmOptions = new AndroidFcmOptions
                    {
                        AnalyticsLabel = "Finalizada"
                    }
                };
                message.Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Alert = new ApsAlert
                        {
                            TitleLocKey = "asistencia_finalizada_title",
                            LocKey = "asistencia_finalizada_body",
                        },
                    },
                    FcmOptions = new ApnsFcmOptions
                    {
                        AnalyticsLabel = "Finalizada"
                    }
                };

                // Send a message to the device corresponding to the provided
                // registration token.
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                // Response is a message ID string.
                Console.WriteLine("AlertaFinalizarEventHandler Successfully sent message: " + response);
            }
            catch (Exception ex)
            {
                Log.Get().Error("Error en el evento AlertaFinalizarEventHandler", ex);
            }
        }
    }
}