using FirebaseAdmin.Messaging;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Log;
using UCare.Domain.Users.Events;

namespace UCare.Web.EventsHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public class NuevaComunicacionEventHandler : IDomainEventSubscriber<NuevaComunicacionEvent>
    {
        /// <summary>
        /// 
        /// </summary>
        public NuevaComunicacionEventHandler()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        public async Task On(NuevaComunicacionEvent domainEvent)
        {
            try
            {
                // See documentation on defining a message payload.
                var message = new Message();

                message.Notification = new Notification
                {
                    Title = domainEvent.Titulo,
                    Body = domainEvent.Mensaje,
                };
                message.Topic = $"{domainEvent.AggregateId}";

                message.Data = new Dictionary<string, string>()
                {
                    { "cominucacionId", domainEvent.ComunicacionId },
                };

                message.FcmOptions = new FcmOptions
                {
                    AnalyticsLabel = "Comunicacion"
                };


                // Send a message to the device corresponding to the provided
                // registration token.
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                // Response is a message ID string.
                Console.WriteLine("NuevaComunicacionEventHandler Successfully sent message: " + response);
            }
            catch (Exception ex)
            {
                Log.Get().Error("Error en el evento NuevaComunicacionEventHandler", ex);
            }
        }
    }
}