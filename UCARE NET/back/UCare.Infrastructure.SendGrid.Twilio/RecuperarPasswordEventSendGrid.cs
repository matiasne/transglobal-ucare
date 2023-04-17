using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Core.service;
using GQ.Log;
using SendGrid;
using SendGrid.Helpers.Mail;
using UCare.Domain.Users;
using UCare.Domain.Users.Events;

namespace UCare.Infrastructure.SendGrid.Twilio
{
//# ------------------
//# Create a campaign \
//# ------------------
//curl -H 'api-key:YOUR_API_V3_KEY' \
//-X POST -d '{ \
//# Define the campaign settings \
//"name":"Campaign sent via the API", \
//"subject":"My subject", \
//"sender": { "name": "From name", "email":"ucare-server@transglobal-technologies.com" }, \
//"type": "classic", \
//# Content that will be sent \
//"htmlContent": "Congratulations! You successfully sent this example campaign via the Sendinblue API.", \
//# Select the recipients\
//"recipients": { "listIds": [2,7] }, \
//# Schedule the sending in one hour\
//"scheduledAt": "2018-01-01 00:00:01", \
//}'
//'https://api.sendinblue.com/v3/emailCampaigns'
    public class RecuperarPasswordEventSendGrid : IDomainEventSubscriber<RecuperarPasswordEvent>
    {
        private readonly IUsuarioManagerRepository repository;
        public RecuperarPasswordEventSendGrid(IUsuarioManagerRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// https://localhost:44401/recuperar?id=eUDCDnYRplrH080bBvv1|b2c915cc-09fc-4b72-8e97-d0d7d9089fb2
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <returns></returns>
        public async Task On(RecuperarPasswordEvent domainEvent)
        {
            try
            {
                var user = await repository.GetById(domainEvent.AggregateId.Split("|")[0]);
                if (user != null)
                {
                    var apiKey = ServicesContainer.Configuration["Mail:SendGrid:Apikey"] ?? "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";
                    var fromEmail = ServicesContainer.Configuration["Mail:FromEmail"] ?? "esteban.yofre@geminus-qhom.com";
                    var fromName = ServicesContainer.Configuration["Mail:FromName"] ?? "Esteban Yofre";

                    var client = new SendGridClient(apiKey);
                    var from = new EmailAddress(fromEmail, fromName);
                    var to = new EmailAddress(user.Email, user.UsuarioNombre);

                    var subject = ServicesContainer.Configuration["Mail:Templates:Recuperacion:Subject"] ?? "Recuperacion de contraseña UCARE";
                    var plainTextContent = ServicesContainer.Configuration["Mail:Templates:Recuperacion:PlainContent"] ?? "Su codigo de verificacion es {5}. Para recuperar la contrseña haga click en el siguiente link {0}";
                    var htmlContent = ServicesContainer.Configuration["Mail:Templates:Recuperacion:HtmlContent"] ?? "Su codigo de verificacion es <h2><b>{5}</b></h2>. Para recuperar la contrseña haga click en el siguiente link <a href='{0}'>Recuperar</a>";

                    var urlBase = $"{(ServicesContainer.Configuration["UrlBase"] ?? "https://test.geminus-qhom.com/ucare-test")}/recuperar?id={domainEvent.AggregateId}";

                    subject = String.Format(subject, urlBase, user.Id, user.UsuarioNombre, user.Email, domainEvent.AggregateId, domainEvent.Code, domainEvent.Expiration);
                    plainTextContent = String.Format(plainTextContent, urlBase, user.Id, user.UsuarioNombre, user.Email, domainEvent.AggregateId, domainEvent.Code, domainEvent.Expiration);
                    htmlContent = String.Format(htmlContent, urlBase, user.Id, user.UsuarioNombre, user.Email, domainEvent.AggregateId, domainEvent.Code, domainEvent.Expiration);

                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                    var response = await client.SendEmailAsync(msg);

                    Log.Get().Warn($"Envio SendGrid status : {response.StatusCode} Message :{response.Body} ");
                }
            }
            catch (Exception ex)
            {
                Log.Get().Error("Error en el envio de SendGrid", ex);
            }
        }
    }
}