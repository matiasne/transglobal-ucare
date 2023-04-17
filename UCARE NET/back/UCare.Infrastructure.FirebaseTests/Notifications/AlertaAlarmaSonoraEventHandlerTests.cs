using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using GQ.Core.service;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCare.Domain.Alertas.Events;

namespace UCare.Web.EventsHandlers.Tests
{
    [TestClass()]
    public class AlertaAlarmaSonoraEventHandlerTests
    {
        private readonly string Config = @"{
  ""type"": ""service_account"",
  ""project_id"": ""tgt-geminus-test"",
  ""private_key_id"": ""bf208546dc10e0eedf2aa4e632f7f58c2c7302b6"",
  ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQCn0e0BhVop/MFg\n1JIMc9NUmQRhW/d2ELCUiRN0hp4IZnvinveiNtSJ65Ca02psbjRRb9uQkID7uOtN\n5X6WTrp4DN+NRj1O6e19vXhUhxIF0k4OPiGsI3fsXGs+x7SRpGnWA/rifqGVbilZ\n+pgcsID2DC8tQS9CTkvZ2VdcVHmrrwo+Vnr6pT7b6V53GD9Y+sEPyg2nKR/t3qXZ\nrGJDGDa5i/9J2ACP70Pm+V0YQWuthlwEa7z78/5maivrfI8/lHUi2CeuOFa4AyXD\nA5WM06gWvRfC63d/Cq3RfurZyjx8lQoblAdKiNwv42pKPDxwOh1WuyxJYo0WoTcE\nH49GAdSzAgMBAAECggEAI0epFRxFL7WAq8wUZgTFhS/j8VnFpK7uiFNm4STZvTda\nTPKRaaswNExu6nWOlnv2iLZEyNfwHbsq0IXx/kRPxCQkrQz6n4fjEv+Nd+urWMj0\n+t6J8qfEMs7dBuYx6jpPu5u7lvj7HDB8Bfv+FW3vtooqMs/U0RIntQURbgoeR/6d\nqu9SuOSDiYhGYgqk4EyyxwhYcIdMvVQedsQ+GE5pELCc88te4hagQn1Nd/9hekTa\nJbR9m37PD7FpcBpBv9HSN+zsIFOB2YGhKr6gaHIVqSlUCTwDk/U3VZR8ltOBvGNs\nHyeB3wZV1XEYht5D0NNLw0HRQqWVyxvTCZmXJmuFlQKBgQDWxeG0dob/crs5A4lR\nbTfqbMGPIB9PNC3KRuCoTp2+IRIXzScg8JSDvghMwaC3kx4NQXCsa2LwhPvaOY0H\nPKj9rsNg8RjcFaTuZg2qQbpdqmSnOOLIwnzyzh4RT9VhDqtSjtoEr7eQS9pa1G9o\nPKd8y1pYzXDRIzQrDkPB37XMhwKBgQDICLzp0OZbd6PIyC9gxXIVeTue1Ttr8uUo\naHbjVYIlvbDO+v1I4BvzGP97xK034uGYyZCsW4dV1ebjkKmo/cWDoPOi2EuT7Dpc\naB9S9v3v/n6jdZWmpSB1m4qnZsfj0hSOKxl6nnSisjYxu8e02CSNMPrmTFCQQ2fg\n5Be/1dONdQKBgDsMGGunEqJ0hqi8Iiqy3majE+wnprP94oD0T5u2UYQOT40fqBxU\nSGCYBGwl5+vQlLiMP1AIDiGWTi/HFtDgio2EWM70OiN4B+pLQIKIo5ZQ3G9lImST\npAqVRIr56e5PPCbsg2A6dztCv3utYBvGYT3cIrC7esLk7NjRiStqN+9NAoGAZyD6\nJKHq4aVg75cltgKVurRyJIVSyWjE9HxHLxVasBKIIW4NP5ErV72/mhPgKjvyi08A\nI5xcvcg17YUbs2CCgEZu1bol3PGhdRrJa4CjkyPmLmfk67GedoPmjD/VNoOMzW8z\nB6DCCU9P2Xwyje4RiimCcVFPB9F4sN1n0SNwmjECgYBeQe2mEXlSQcBF9ptMMrix\nrWml6fca1Y/XTTzh2m6k92b+2QUdMzIV+f74yNe+9Pj4oeZztcbRIsfBE6o/M8Sm\n61tfFwGfAjb9FGUFG6KLTBxwSFDuMQtD1xzp86M11YYMfUXGiThfQIoTVTXQSI/v\nIKbU3ZROeezro0LXnJX9jw==\n-----END PRIVATE KEY-----\n"",
  ""client_email"": ""firebase-adminsdk-vl9ve@tgt-geminus-test.iam.gserviceaccount.com"",
  ""client_id"": ""110957169344517062465"",
  ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
  ""token_uri"": ""https://oauth2.googleapis.com/token"",
  ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
  ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-vl9ve%40tgt-geminus-test.iam.gserviceaccount.com""
}";
        public void Iniciar()
        {
            try
            {
                IHostEnvironment HostEnvironment = new HostingEnvironment();
                HostEnvironment.EnvironmentName = "Test";
                HostEnvironment.ContentRootPath = this.GetType().Assembly.Location.Substring(0, this.GetType().Assembly.Location.LastIndexOf('\\'));
                ServicesContainer.AddHostingEnvironment(HostEnvironment);
                var Configuration = ServicesContainer.ConfigurationBuilder();
                ServicesContainer.AddHost(Host.CreateDefaultBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   ServicesContainer.AddServices(services);
               }).Build());

                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(Config)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [TestMethod()]
        public async Task OnTest()
        {
            Iniciar();
            string esteban = "2gOXSWHKOBAhjxEDO6Se";
            string guillermo = "xbymXBoD5t8kpaeHS9VT";
            string gonza = "GeYzWBy0BbCBPdekx61d";

            string send = esteban;

            var evento = new AlertaAlarmaSonoraEvent(send, send, true);
            var handler = new AlertaAlarmaSonoraEventHandler();
            await handler.On(evento);

            await Task.Delay(5000);

            evento = new AlertaAlarmaSonoraEvent(send, send, false);
            handler = new AlertaAlarmaSonoraEventHandler();
            await handler.On(evento);

            // Se requiere comprobacion visual en el dispositivo
            Assert.IsTrue(true);
        }
    }
}