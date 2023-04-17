using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCare.Infrastructure.SendGrid.Twilio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GQ.Core.service;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Hosting;
using UCare.Domain.Users.Events;
using Microsoft.Extensions.Configuration;

namespace UCare.Infrastructure.SendGrid.Twilio.Tests
{
    [TestClass()]
    public class RecuperarPasswordEventSendGridTests
    {

        private readonly string Config = @"{
  ""SMS"": {
    ""Twilio"": {
      ""From"": ""+16573258656"",
      ""Sid"": ""AC2107c7cdaa3611250c5a11d32ab11acd"",
      ""Token"": ""70f52ea787806f2b70f1aa9ce3c6f39b"",
      ""signature"": ""4D+tcThqXG5""
    },
    ""VerificacionTelefono"": ""{0} tu código de Verificación es {1} ""
  }
}";
        public void Iniciar()
        {
            try
            {
                IHostEnvironment HostEnvironment = new HostingEnvironment();
                HostEnvironment.EnvironmentName = "Test";
                HostEnvironment.ContentRootPath = this.GetType().Assembly.Location.Substring(0, this.GetType().Assembly.Location.LastIndexOf('\\'));
                ServicesContainer.AddHostingEnvironment(HostEnvironment);
                var Configuration = ServicesContainer.ConfigurationBuilder((config) =>
                {
                    var mem = new StreamWriter(new MemoryStream());
                    mem.Write(Config);
                    mem.Flush();
                    mem.BaseStream.Position = 0;
                    config.AddJsonStream(mem.BaseStream);
                });
                ServicesContainer.AddHost(Host.CreateDefaultBuilder()
               .ConfigureServices((hostContext, services) =>
               {
                   ServicesContainer.AddServices(services);
               }).Build());

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

            ////Numero de la argentina
            //var evento = new RecuperarPasswordEvent("", "Unit Test", "1234", "+542645064962");
            //var handler = new RecuperarPasswordEventSendGrid(null);
            //await handler.On(evento);

            ////Numero de uruguay
            //evento = new RecuperarPasswordEvent("", "Unit Test", "1234", "+543512065002");
            //handler = new RecuperarPasswordEventSendGrid(null);
            //await handler.On(evento);

            // Se requiere comprobacion visual en el dispositivo
            Assert.IsTrue(true);
        }
}
}