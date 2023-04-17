using AutoMapper;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using GQ.Architecture.DDD.Domain.Repository;
using GQ.Log;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UCare.Domain.Alertas;
using UCare.Domain.Alertas.Events;
using UCare.Domain.Comunicaciones;
using UCare.Domain.Users;
using UCare.Domain.Users.Events;
using UCare.Infrastructure.Firebase.Alertas;
using UCare.Infrastructure.Firebase.Comunicaciones;
using UCare.Infrastructure.Firebase.Users;
using UCare.Shared.Domain;
using UCare.Web.EventsHandlers;

namespace UCare.Infrastructure.Firebase
{
    public static class DependencyInjection
    {
        public static List<T> ConvertTo<T>(this QuerySnapshot query)
        {
            var list = new List<T>();
            try
            {
                foreach (var item in query)
                {
                    list.Add(item.ConvertTo<T>());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return list;
        }

        public static List<TDomain> ConvertTo<TDomain, TFirebase>(this QuerySnapshot query, Mapper mapper)
        {
            var list = new List<TDomain>();
            try
            {
                foreach (var item in query)
                {
                    list.Add(mapper.Map<TDomain>(item.ConvertTo<TFirebase>()));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return list;
        }

        public static void AddDatabase(this IServiceCollection service, IConfiguration configuration, string ContentRootPath)
        {
            var proyectId = configuration["ConnectionString:ProyectId"] ?? "stellar-river-359012";
            var credentials = configuration["ConnectionString:Credentials"] ?? "firebase.json";

            Console.WriteLine($"proyectId: {proyectId} | credentials:{credentials} | path : {Path.Combine(ContentRootPath, credentials)}");

            Task.Delay(4000).ContinueWith((t) =>
            {
                try
                {
                    Log.Get().Info($"proyectId: {proyectId} | credentials:{credentials} | path : {Path.Combine(ContentRootPath, credentials)}");
                }
                catch { }
            });

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("GOOGLE_APPLIC:ATION_CREDENTIALS")))
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Path.Combine(ContentRootPath, credentials));
            }

            service.AddScoped((serviceProvider) =>
            {
                return FirestoreDb.Create(proyectId);
            });

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(ContentRootPath, credentials))
            });

            service.AddRepository(typeof(DependencyInjection).Assembly);

            var configurationMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Config.ConfigFirebase, UCare.Domain.Config.Config>();
                cfg.CreateMap<UCare.Domain.Config.Config, Config.ConfigFirebase>();

                cfg.CreateMap<Alerta, AlertaFirebase>();
                cfg.CreateMap<AlertaFirebase, Alerta>();

                cfg.CreateMap<AlertaHistorial, AlertaHistorialFirebase>();
                cfg.CreateMap<AlertaHistorialFirebase, AlertaHistorial>();

                cfg.CreateMap<VerificacionFirebase, Verificacion>();
                cfg.CreateMap<Verificacion, VerificacionFirebase>();

                cfg.CreateMap<UsuarioAfiliado, UsuarioAfiliadoFirebase>();
                cfg.CreateMap<UsuarioAfiliadoFirebase, UsuarioAfiliado>();

                cfg.CreateMap<ComunicacionAfiliado, ComunicacionAfiliadoFirebase>();
                cfg.CreateMap<ComunicacionAfiliadoFirebase, ComunicacionAfiliado>();

                cfg.CreateMap<UsuarioManager, UsuarioManagerFirebase>();
                cfg.CreateMap<UsuarioManagerFirebase, UsuarioManager>();

                cfg.CreateMap<UsuarioManagerHistorial, UsuarioManagerHistorialFirebase>();
                cfg.CreateMap<UsuarioManagerHistorialFirebase, UsuarioManagerHistorial>();

                cfg.CreateMap<UsuarioAuth, UsuarioAuthFirebase>();
                cfg.CreateMap<UsuarioAuthFirebase, UsuarioAuth>();

                cfg.CreateMap<Direccion, DireccionFirebase>();
                cfg.CreateMap<DireccionFirebase, Direccion>();

                cfg.CreateMap<GeoPositionFirebase, GeoPosition>();
                cfg.CreateMap<GeoPosition, GeoPositionFirebase>();

                cfg.CreateMap<Contacto, ContactoFirebase>();
                cfg.CreateMap<ContactoFirebase, Contacto>();

                cfg.CreateMap<Afiliacion, AfiliacionFirebase>();
                cfg.CreateMap<AfiliacionFirebase, Afiliacion>();

                cfg.CreateMap<Modificacion, ModificacionFirebase>();
                cfg.CreateMap<ModificacionFirebase, Modificacion>();

                cfg.CreateMap<MapaConfig, MapaConfigFirebase>();
                cfg.CreateMap<MapaConfigFirebase, MapaConfig>();

                cfg.CreateMap<Comunicacion, ComunicacionFirebase>();
                cfg.CreateMap<ComunicacionFirebase, Comunicacion>();

                cfg.CreateMap<ComunicacionEnvio, ComunicacionEnvioFirebase>();
                cfg.CreateMap<ComunicacionEnvioFirebase, ComunicacionEnvio>();
            });

            configurationMapper.AssertConfigurationIsValid();

            service.AddSingleton<Mapper>((sp) =>
            {
                return new Mapper(configurationMapper);
            });

            //Eventos
            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<AlertaAlarmaSonoraEvent>, AlertaAlarmaSonoraEventHandler>();
            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<NuevaComunicacionEvent>, NuevaComunicacionEventHandler>();
            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<AlertaFinalizarEvent>, AlertaFinalizarEventHandler>();
            service.AddTransient<GQ.Architecture.DDD.Domain.Bus.Event.IDomainEventSubscriber<ChangeEstadoEvent>, ChangeEstadoEventHandler>();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepository(this IServiceCollection service, Assembly assembly)
        {
            return AddRepository(service, () => GQ.Architecture.DDD.DependencyInjection.GetClassToInterface(assembly, typeof(IRepository)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepository(this IServiceCollection service, Func<IEnumerable<Type>> func)
        {
            var classTypes = func();
            foreach (var type in classTypes)
            {
                service.AddScoped(type.GetInterfaces()[0], type);
            }
            return service;
        }
    }
}
