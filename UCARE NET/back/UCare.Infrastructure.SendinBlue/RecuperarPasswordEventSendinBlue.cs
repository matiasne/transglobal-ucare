using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Core.service;
using GQ.Log;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using UCare.Domain.Users;
using UCare.Domain.Users.Events;

namespace UCare.Infrastructure.SendinBlue
{
    public class RecuperarPasswordEventSendinBlue : IDomainEventSubscriber<RecuperarPasswordEvent>
    {
        private readonly IUsuarioManagerRepository repository;
        private readonly IConfiguration configuration;
        public RecuperarPasswordEventSendinBlue(IUsuarioManagerRepository repository)
        {
            this.repository = repository;
            this.configuration = configuration;
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
                    var fromEmail = ServicesContainer.Configuration["Mail:FromEmail"] ?? "ucare-server@transglobal-technologies.com";
                    var fromName = ServicesContainer.Configuration["Mail:FromName"] ?? "noreplay";

                    var subject = ServicesContainer.Configuration["Mail:Templates:Recuperacion:Subject"] ?? "Recuperacion de contraseña UCARE";
                    //var plainTextContent = ServicesContainer.Configuration["Mail:Templates:Recuperacion:PlainContent"] ?? "Su codigo de verificacion es {5}. Para recuperar la contrseña haga click en el siguiente link {0}";
                    var htmlContent = ServicesContainer.Configuration["Mail:Templates:Recuperacion:HtmlContent"] ?? "Su codigo de verificacion es <h2><b>{5}</b></h2>. Para recuperar la contrseña haga click en el siguiente link <a href='{0}'>Recuperar</a>";

                    var urlBase = $"{(ServicesContainer.Configuration["UrlBase"] ?? "https://test.geminus-qhom.com/ucare-test")}/recuperar?id={domainEvent.AggregateId}";

                    subject = String.Format(subject, urlBase, user.Id, user.UsuarioNombre, user.Email, domainEvent.AggregateId, domainEvent.Code, domainEvent.Expiration);
                    //plainTextContent = String.Format(plainTextContent, urlBase, user.Id, user.UsuarioNombre, user.Email, domainEvent.AggregateId, domainEvent.Code, domainEvent.Expiration);
                    htmlContent = String.Format(htmlContent, urlBase, user.Id, user.UsuarioNombre, user.Email, domainEvent.AggregateId, domainEvent.Code, domainEvent.Expiration);


                    HttpClient client = new HttpClient();
                    string contentType = "application/json";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                    client.DefaultRequestHeaders.Add("api-key", ServicesContainer.Configuration["Mail:SendinBlue:ApiKey"]?? "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");
                    var bodyContent = JsonConvert.SerializeObject(new EmailSend
                    {
                        Sender = new EmailAdress { Name = fromName, Email = fromEmail },
                        To = new EmailAdress[] { new EmailAdress { Name = user!.UsuarioNombre, Email = user!.Email } },
                        Subject = subject,
                        HtmlContent = htmlContent
                    });
                    HttpContent content = new StringContent(bodyContent);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync("https://api.sendinblue.com/v3/smtp/email", content);
                    var body = new StreamReader(response.Content.ReadAsStream()).ReadToEnd();
                    Log.Get().Warn($"Envio SendGrid status : {response.StatusCode} Message :{body} ");
                }
            }
            catch (Exception ex)
            {
                Log.Get().Error("Error en el envio de SendGrid", ex);
            }
        }
    }
    public class EmailAdress
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("email")]
        public string? Email { get; set; }
    }
    public class EmailSend
    {
        [JsonProperty("sender")]
        public EmailAdress Sender { get; set; }
        [JsonProperty("to")]
        public EmailAdress[] To { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("htmlContent")]
        public string HtmlContent { get; set; }
    }
}