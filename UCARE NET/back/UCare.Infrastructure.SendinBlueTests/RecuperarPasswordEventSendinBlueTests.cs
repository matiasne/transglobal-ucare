using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCare.Infrastructure.SendinBlue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GQ.Core.service;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using UCare.Domain.Users.Events;
using UCare.Domain.Users;
using UCare.Shared.Infrastructure;

namespace UCare.Infrastructure.SendinBlue.Tests
{
    [TestClass()]
    public class RecuperarPasswordEventSendinBlueTests
    {
        private readonly string Config = @"{
  ""Mail"": {
    ""FromEmail"": ""ucare-server@transglobal-technologies.com"",
    ""FromName"": ""noreplay"",
    ""SendinBlue"": {
      ""ApiKey"": ""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""
    },
    ""Templates"": {
      ""Recuperacion"": {
        ""Subject"": ""Recuperacion de contraseña"",
        ""PlainContent"": ""Para recuperar la contrseña haga click en el siguiente link {0}"",
        ""HtmlContent"": ""Para recuperar la contrseña haga click en el siguiente link <a href='{0}'>Recuperar</a>""
      }
    }
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
        public void OnTest()
        {
            Iniciar();
            var ev = new RecuperarPasswordEvent("eUDCDnYRplrH080bBvv1", "M", "esteban", "1234", DateTime.Now, "1234");
            new RecuperarPasswordEventSendinBlue(new Repo() ).On(ev).Wait();
            Assert.IsTrue(true);

        }

    }
    public class Repo : IUsuarioManagerRepository
    {
        public Task Delete(string? id)
        {
            throw new NotImplementedException();
        }

        public Task<IPaging> Get(IPaging paging)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsuarioManager>> GetAllUserManager()
        {
            throw new NotImplementedException();
        }

        public Task<List<UsuarioAuth>> GetAuthByUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioManager> GetById(string? id)
        {
            return Task.FromResult(new UsuarioManager { Id = "eUDCDnYRplrH080bBvv1", UsuarioNombre = "esteban", Email = "beastsoft@gmail.com" });
        }

        public Task<List<UsuarioManager>> GetByRol(string rol)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioManager?> GetByUserEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioManager?> GetByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsuarioManager>> GetUsersByIdRol(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UsuarioManager>> GetUsersByUserId(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioManager> Insert(UsuarioManager entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertAu(UsuarioAuth au)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegistrarHistorial(string id, UsuarioManagerHistorial entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAuthByUser(string id, string authId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(UsuarioManager model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMapaConfig(string id, MapaConfig entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePassword(UsuarioManager user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUsuarioId(UsuarioManager entity)
        {
            throw new NotImplementedException();
        }
    }
}