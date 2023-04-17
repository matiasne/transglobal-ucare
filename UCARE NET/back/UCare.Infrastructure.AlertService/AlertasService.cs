using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Log;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UCare.Application.Alertas;
using UCare.Domain.Alertas;
using UCare.Domain.Alertas.Events;
using UCare.Domain.Config;
using UCare.Domain.Users;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure.AlertasService;

namespace UCare.Infrastructure.AlertService
{
    public class AlertasService<THub> : IAlertasService where THub : Hub
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration config;
        private readonly List<Monitor> monitorStates = new List<Monitor>();
        private readonly List<AlertaContent> alertasSinAsignar = new List<AlertaContent>();
        private readonly List<AlertaContent> alertasAsignadas = new List<AlertaContent>();

        //Blockea los hilos al momneto de trabajar con las alertas para ser asignadas
        private readonly Semaphore alertasLock = new Semaphore(1, 1);

        private bool isRunning = false;
        private bool isEnd = true;
        private Task? task;

        public Config Config { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hubClients"></param>
        /// <param name="app"></param>
        public AlertasService(IServiceProvider serviceProvider, IConfiguration config)
        {
            try
            {
                this.serviceProvider = serviceProvider;
                this.config = config;

                using var scope = serviceProvider.CreateScope();
                var appConfig = scope.ServiceProvider.GetService<IConfigRepository>()!;
                Config = appConfig.GetById("").Result;

                var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;
                var alertas = app.GetPendientes().Result;
                foreach (var item in alertas)
                {
                    alertasSinAsignar.Add(AlertaContent.Create(item));
                }

                alertas = app.GetPendientesAsignados().Result;
                foreach (var item in alertas)
                {
                    alertasAsignadas.Add(AlertaContent.Create(item));
                }
                Start();
            }
            catch (Exception e)
            {
                Log.Get().Error("AlertasService", e);
                throw;
            }
        }

        private long GetMonitorTimeOut(bool addNow = true)
        {
            long seconds;
            if (!long.TryParse(config["AlertService:MonitorTimeOut"] ?? "10", out seconds))
            {
                seconds = 10;
            }
            if (addNow) return DateTime.UtcNow.AddSeconds(Config.MonitorPausaTimeOut ?? seconds).Ticks;
            else return Config.MonitorPausaTimeOut ?? seconds;
        }

        private long GetConfirmarTimeOut(bool addNow = true)
        {
            long seconds;
            if (!long.TryParse(config["AlertService:ConfirmarTimeOut"] ?? "60", out seconds))
            {
                seconds = 60;
            }
            if (addNow) return DateTime.UtcNow.AddSeconds(Config.ConfirmarTimeOut ?? seconds).Ticks;
            else return Config.ConfirmarTimeOut ?? seconds;
        }

        private long GetTiempoParaReasignarAlerta(bool addNow = true)
        {
            long seconds;
            if (!long.TryParse(config["AlertService:TiempoParaReasignarAlerta"] ?? "600", out seconds))
            {
                seconds = 600;
            }
            if (addNow) return DateTime.UtcNow.AddSeconds(Config.TiempoParaReasignarAlerta ?? seconds).Ticks;
            else return Config.TiempoParaReasignarAlerta ?? seconds;
        }

        private long GetTiempoEnvioSMSSeconds(bool addNow = true)
        {
            long seconds;
            if (!long.TryParse(config["AlertService:TiempoEnvioSMSSeconds"] ?? "600", out seconds))
            {
                seconds = 600;
            }
            if (addNow) return DateTime.UtcNow.AddSeconds(Config.TiempoEnvioSMSSeconds ?? seconds).Ticks;
            else return Config.TiempoEnvioSMSSeconds ?? seconds;
        }

        public void Start()
        {
            isRunning = true;
            if (task == null || task.IsCompleted || task.IsCanceled || task.IsFaulted)
            {
                RunService();
            }
        }

        public void Stop()
        {
            isRunning = false;
        }

        /// <summary>
        /// Metodo que se ejecuta por cada alerta nueva quese cree en el sistema -
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> AddAlerta(string id)
        {
            using var scope = serviceProvider.CreateScope();
            var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;
            var model = await app.GetAlertaById(id);
            addAlertInternal(AlertaContent.Create(model));
            await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.All.SendAsync("OnAddNewAlerta", model);
            return true;
        }

        /// <summary>
        /// Agregamos una alerta
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="control">Valor por defecto true si esta en true controla que otro hilo no este intentando acceder al objeto</param>
        private void addAlertInternal(AlertaContent alert, bool control = true)
        {
            if (control)
                alertasLock.WaitOne();
            alertasSinAsignar.Add(alert);
            if (control)
                alertasLock.Release();
        }

        /// <summary>
        /// Removemos la alerta
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="control">Valor por defecto true si esta en true controla que otro hilo no este intentando acceder al objeto</param>
        private void removeAlertInternal(AlertaContent alert, bool control = true)
        {
            if (control)
                alertasLock.WaitOne();
            alertasSinAsignar.Remove(alert);
            if (control)
                alertasLock.Release();
        }


        /// <summary>
        /// Runita principal del servicio
        /// </summary>
        private void RunService()
        {
            if (isEnd)
            {
                isEnd = false;
                task = new Task(async () =>
                {
                    while (isRunning)
                    {
                        var now = DateTime.UtcNow;
                        //Chequeamos que halla alertas sin tomar y monitores libres
                        if (alertasSinAsignar.Count > 0 && monitorStates.Any(x => x.State == MonitorState.Waiting && x.TimeOut < now.Ticks))
                        {
                            try
                            {
                                alertasLock.WaitOne();

                                //Buscamos a todos los monitores libres que ordenados por ultima modificacion de estado
                                var monotoresLibres = monitorStates.Where(x => x.State == MonitorState.Waiting && x.TimeOut < now.Ticks).OrderBy(x => x.Ticks).ToList();

                                foreach (var monotores in monotoresLibres)
                                {
                                    var cp = monotores.GetCodigoPostal();

                                    AlertaContent? alerta = alertasSinAsignar.OrderBy(x => x.Creado.Modificado)
                                    //Condicion para asignar una alerta a un monitor
                                    // si la alerta no tiene código postal
                                    // si el monitor no tiene código postal
                                    // si el código postal del monitor coincide con el código postal de la alerta
                                    .FirstOrDefault(x => string.IsNullOrWhiteSpace(x.AfiliadoCodigoPostal) || cp == null || cp.Count == 0 || cp.Any(y => y == x.AfiliadoCodigoPostal));

                                    // Si se encuntra una alerta se la asigna al monitor
                                    if (alerta != null)
                                    {
                                        using var scope = serviceProvider.CreateScope();
                                        var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;

                                        if (await app.Asignar(alerta, monotores!.Id))
                                        {
                                            Log.Get().Info($"Asignacion de Alerta id : {alerta.Id} cp Alerta : {alerta.AfiliadoCodigoPostal} - Monitor id :{monotores.Id} cps Monitor : {string.Join(",", cp ?? new List<string>())}");

                                            //se cambia el estado al monitor
                                            monotores.ChangeState(MonitorState.Asigned);
                                            alerta.TimeOut = GetConfirmarTimeOut();


                                            // Se pasa a la alerta a la lista de alertas asignadas para su control de timeout
                                            alertasAsignadas.Add(alerta);

                                            // Se saca la alerta de la coleccion de alertas sin asignar
                                            removeAlertInternal(alerta, false);

                                            // Se envio notificacion al usuario
                                            await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.Client(monotores.ConnectionId).SendAsync("OnSelectAlerta", alerta);
                                        }
                                        else
                                        {
                                            addAlertInternal(alerta, false);
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Get().Error("Chequeo de asignacion de alertas", e);
                            }
                            finally
                            {
                                alertasLock.Release();
                            }
                        }
                        // Esperamos unos segundos para no saturar el servicio
                        await Task.Delay(2000);

                        try
                        {
                            // Revisamos si las asignaciones fueron confirmadas
                            if (alertasAsignadas.Any(x => x.Cerrado == true))
                            {
                                alertasAsignadas.RemoveAll(x => x.Cerrado == true);
                            }

                            // Chequeamos cuales asignaciones pasaron mas de x tiempo sin ser confirmadas 
                            // y las volvemos a la cola para que sean rea asignadas
                            now = DateTime.UtcNow;
                            if (alertasAsignadas.Any(x => x.TimeOut < now.Ticks))
                            {
                                using var scope = serviceProvider.CreateScope();
                                var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;

                                var alertas = alertasAsignadas.Where(x => x.TimeOut < now.Ticks).ToList();

                                foreach (var alerta in alertas)
                                {
                                    var monitorId = alerta.MonitorId;
                                    if (!(alerta.ConfirmaAsignacion ?? false))
                                    {
                                        //Cuando caduca el timpo de confirmacion asignamso la alerta a otro usuario
                                        //Parra ello primero desasignamos al usuario monitor actual de la alerta
                                        if (await app.Asignar(alerta, null))
                                        {
                                            addAlertInternal(alerta);
                                            alertasAsignadas.Remove(alerta);
                                            await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.All.SendAsync("OnChangeAlerta", alerta);
                                        }

                                        // Si tiene un monitor asignado le enviamos un mensaje que esta pausado 
                                        if (!string.IsNullOrEmpty(monitorId))
                                        {
                                            var monitor = monitorStates.FirstOrDefault(x => x.Id == monitorId);
                                            if (monitor != null && monitor.State == MonitorState.Asigned)
                                            {
                                                monitor.ChangeState(MonitorState.Pause);
                                                await app.RegistrarHistorialMonitor(monitor.Id, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.PAUSAR));
                                                await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.Client(monitor.ConnectionId).SendAsync("OnChangeMonitorStatus", monitor);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Buscamos si el monitor esta conectado
                                        if (monitorStates.Any(x => x.Id == alerta.MonitorId))
                                        {
                                            var monitor = monitorStates.First(x => x.Id == alerta.MonitorId);
                                            // Si el estado todavia sigue asignado o el cambio es menor a las 5 min renueva la alerta
                                            if (monitor.State == MonitorState.Asigned ||
                                            TimeSpan.FromTicks(DateTime.UtcNow.Ticks).Subtract(TimeSpan.FromTicks(monitor.Ticks)).TotalSeconds < (GetTiempoParaReasignarAlerta(false) / 2))
                                            {
                                                alerta.TimeOut = GetTiempoParaReasignarAlerta();
                                                await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.Client(monitor.ConnectionId).SendAsync("OnChangeAlerta", alerta);

                                                //El continue es para que no haga el cambio de monitor
                                                continue;
                                            }
                                        }

                                        //Si no encontramos al usuario logueado lo asignamos a otro monitor
                                        if (await app.Asignar(alerta, null))
                                        {
                                            addAlertInternal(alerta);
                                            alertasAsignadas.Remove(alerta);
                                            await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.All.SendAsync("OnChangeAlerta", alerta);
                                        }
                                    }
                                }
                            }

                            // Notificamos a los contactos de emergencia sobre la emergencia del afiliado
                            if (alertasAsignadas.Any(x => x.Estado == Estados.Emergencia && x.TimeSendSMS != 0 && x.TimeSendSMS < now.Ticks))
                            {
                                var alertas = alertasAsignadas.Where(x => x.Estado == Estados.Emergencia && x.TimeSendSMS != 0 && x.TimeSendSMS < now.Ticks);
                                foreach (var item in alertas)
                                {
                                    using var scope = serviceProvider.CreateScope();
                                    var eventBus = scope.ServiceProvider.GetService<IEventBus>();

                                    await eventBus.Publish(new List<DomainEvent> { new AlertaAvisarContactosEvent(item.Id!, item.AfiliadoId) });

                                    item.TimeSendSMS = 0;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Get().Error("Chequeo de alertas timeout", e);
                        }
                        finally
                        {
                            // Esperamos unos segundos para no saturar el servicio
                            await Task.Delay(2000);
                        }
                    }
                    isEnd = true;
                });
                task.Start();
            }
        }

        /// <summary>
        /// El usuario monitor cambia el estado de la alerta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="alertaId"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public async Task<bool> CambiarEstado(string id, string alertaId, string estado)
        {
            using var scope = serviceProvider.CreateScope();
            var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;

            Alerta? alerta = await app.CambiarEstado(id, alertaId, estado);
            if (alerta != null)
            {
                var alerta1 = alertasAsignadas.First(x => x.Id == alertaId);

                alerta1.Estado = alerta.Estado;
                alerta1.Modificacion = alerta.Modificacion;
                alerta1.MonitorId = alerta.MonitorId;
                alerta1.TimeOut = GetTiempoParaReasignarAlerta();
                alerta1.TimeSendSMS = alerta.Estado == Estados.Emergencia ? GetTiempoEnvioSMSSeconds(true) : 0;

                await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.All.SendAsync("OnChangeAlerta", alerta1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// El usuario monitor acepta la alerta para hacer su seguimiento
        /// </summary>
        /// <param name="monitorId"></param>
        /// <param name="alertaId"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmarAsignacion(string monitorId, string alertaId)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;

                var alerta = alertasAsignadas.FirstOrDefault(x => x.Id == alertaId);
                if (alerta != null && (await app.ConfirmarAsignacion(alerta, monitorId)))
                {
                    alerta.TimeOut = GetTiempoParaReasignarAlerta();
                    await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.All.SendAsync("OnChangeAlerta", alerta);
                }
            }
            catch (Exception ex)
            {
                Log.Get().Error("ConfirmarAsignacion", ex);
            }
            return true;
        }

        /// <summary>
        /// Finalizamos la asistencia de la alerta y escribimos la bitacora
        /// </summary>
        /// <param name="id"></param>
        /// <param name="alertaId"></param>
        /// <param name="bitacora"></param>
        /// <returns></returns>
        public async Task<bool> FinalizarAsistencia(string id, string alertaId, string bitacora)
        {
            using var scope = serviceProvider.CreateScope();
            var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;

            Alerta? alerta = await app.FinalizarAsistencia(id, alertaId, bitacora);
            if (alerta != null)
            {
                var alerta1 = alertasAsignadas.First(x => x.Id == alertaId);
                alerta1.Estado = alerta.Estado;
                alerta1.Modificacion = alerta.Modificacion;
                alerta1.MonitorId = alerta.MonitorId;
                alerta1.Bitacora = alerta.Bitacora;
                alerta1.Cerrado = alerta.Cerrado;
                alerta1.TimeOut = GetTiempoParaReasignarAlerta();

                await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.All.SendAsync("OnChangeAlerta", alerta1);
                if (monitorStates.Any(x => x.Id == id))
                {
                    var monitor = monitorStates.Find(x => x.Id == id)!;
                    if (monitor.State == MonitorState.Asigned)
                    {
                        monitor.SetTimeOut(GetMonitorTimeOut());
                        monitor.ChangeState(MonitorState.Waiting);
                    }
                    await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.Client(monitor.ConnectionId).SendAsync("OnChangeMonitorStatus", monitor);
                    return true;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Activamos o desactivamos la alarma sonora de una alerta
        /// </summary>
        /// <param name="id"></param>
        /// <param name="alertaId"></param>
        /// <returns></returns>
        public async Task<bool> ActivarDesactivarAlarma(string id, string alertaId)
        {
            using var scope = serviceProvider.CreateScope();
            var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;
            var alerta = await app.GetAlertaById(alertaId);
            alerta = await app.ActivarDesactivarAlarma(id, alertaId, !(alerta.AlarmaSonora ?? false));
            if (alerta != null)
            {
                var alerta1 = alertasAsignadas.First(x => x.Id == alertaId);
                alerta1.Estado = alerta.Estado;
                alerta1.Modificacion = alerta.Modificacion;
                alerta1.MonitorId = alerta.MonitorId;
                alerta1.Bitacora = alerta.Bitacora;
                alerta1.Cerrado = alerta.Cerrado;
                alerta1.AlarmaSonora = alerta.AlarmaSonora;
                alerta1.TimeOut = GetTiempoParaReasignarAlerta();

                await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.All.SendAsync("OnChangeAlerta", alerta1);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Cuando se Conecta un monitor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connectionId"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ConnectedMonitor(string? id, string connectionId)
        {
            using var scope = serviceProvider.CreateScope();
            var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;
            app.RegistrarHistorialMonitor(id, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.CONECTARSE)).Wait();
        }

        /// <summary>
        /// Cuando se desconecta un monitor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connectionId"></param>
        public void DisconnectedMonitor(string id, string connectionId)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;
                app.RegistrarHistorialMonitor(id, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.DESCONECTARSE)).Wait();

                if (monitorStates.Any(x => x.Id == id))
                {
                    var monitor = monitorStates.Find(x => x.Id == id)!;
                    monitor.ChangeState(MonitorState.Disconected);
                }
            }
            catch (Exception ex)
            {
                Log.Get().Error("DisconnectedMonitor", ex);
            }
        }

        /// <summary>
        /// Preguntamos si ese monitor esta conectado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> IsJoin(string id)
        {
            try
            {
                var now = DateTime.UtcNow.AddMinutes(-1).Ticks;
                return Task.FromResult(monitorStates.Any(x => x.Id == id && x.Ticks > now));
            }
            catch (Exception ex)
            {
                Log.Get().Error("IsJoin", ex);
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// Entra el monitor a la sala
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public async Task<bool> AddMonitor(string id, string connectionId)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;
                var result = await app.GetPendientesAsignados(id);

                Monitor monitor = new Monitor(id, connectionId, state: (result?.Any(x => !(x.Cerrado ?? false)) ?? false) ? MonitorState.Asigned : MonitorState.Waiting);
                monitor.SetTimeOut(GetMonitorTimeOut());
                monitor.SetCodigoPostal(await app.GetCodigosPostales(id));
                if (!monitorStates.Any(x => x.Id == id))
                {
                    monitorStates.Add(monitor);
                }
                else
                {
                    var monitor1 = monitorStates.Find(x => x.Id == id)!;
                    monitor1.SetCodigoPostal(monitor.GetCodigoPostal());
                    monitor1.ChangeState((result?.Any(x => !(x.Cerrado ?? false)) ?? false) ? MonitorState.Asigned : (monitor1.State));
                    await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.Client(monitor1.ConnectionId).SendAsync("OnDisconectedMonitor", monitor);
                    monitor1.SetConnectionId(connectionId);
                    monitor = monitor1;
                }
                await app.RegistrarHistorialMonitor(id, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.ENTRAR));
                await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.Client(monitor.ConnectionId).SendAsync("OnChangeMonitorStatus", monitor);
                return true;
            }
            catch (Exception ex)
            {
                Log.Get().Error("AddMonitor", ex);
            }
            return false;
        }

        /// <summary>
        /// Se retira el monitor de la sala
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> RemoveMonitor(string id)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;
                app.RegistrarHistorialMonitor(id, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.SALIR)).Wait();

                if (monitorStates.Any(x => x.Id == id))
                {
                    monitorStates.Remove(monitorStates.Find(x => x.Id == id)!);
                }
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Log.Get().Error("RemoveMonitor", ex);
            }
            return Task.FromResult(false);
        }


        /// <summary>
        /// El monitor se pide un descanso para no recibir mas alertas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Descanso(string id)
        {
            using var scope = serviceProvider.CreateScope();

            if (monitorStates.Any(x => x.Id == id))
            {
                var monitor = monitorStates.Find(x => x.Id == id)!;
                monitor.ChangeState(MonitorState.Pause);

                var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;
                app.RegistrarHistorialMonitor(id, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.PAUSAR)).Wait();

                await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.Client(monitor.ConnectionId).SendAsync("OnChangeMonitorStatus", monitor);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Inicia la recepcion de alertas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Iniciar(string id)
        {
            using var scope = serviceProvider.CreateScope();

            if (monitorStates.Any(x => x.Id == id))
            {
                var monitor = monitorStates.Find(x => x.Id == id)!;

                var app = scope.ServiceProvider.GetService<AlertasByServiceApp>()!;
                // cada vez que el usuario da iniciar volvemos a buscar los códigos postales por las dudas que cambiaran
                monitor.SetCodigoPostal(await app.GetCodigosPostales(id));

                monitor.ChangeState(MonitorState.Waiting);
                monitor.SetTimeOut(0);

                app.RegistrarHistorialMonitor(id, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.ENTRAR)).Wait();

                await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.Client(monitor.ConnectionId).SendAsync("OnChangeMonitorStatus", monitor);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Si cambiamos las configuracion del sistema hacemos que se actualize
        /// </summary>
        public void ChangeConfig()
        {
            using var scope = serviceProvider.CreateScope();
            var appConfig = scope.ServiceProvider.GetService<IConfigRepository>()!;
            Config = appConfig.GetById("").Result;
        }

        /// <summary>
        /// Obtenemos las alertas asignadas a un monitor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<dynamic>> GetAlertaByUser(string id)
        {
            return Task.FromResult(alertasAsignadas.Where(x => x.MonitorId == id && x.Cerrado == false).ToList<dynamic>());
        }

        public async Task UpdatePosition(string alertaId, double lat, double lon)
        {
            try
            {
                var alerta = alertasSinAsignar.Find(x => x.Id == alertaId);
                if (alerta == null)
                    alerta = alertasAsignadas.Find(x => x.Id == alertaId);
                if (alerta != null)
                {
                    //Actualizamos la posision de la alerta en los objetos en memoria
                    alerta.Position = new GeoPosition { Lat = lat, Lon = lon };
                }
                if (!string.IsNullOrWhiteSpace(alerta.MonitorId))
                {
                    // enviamos la acutualizacion de la posision a la pantalla del monitor
                    var monitor = monitorStates.Find(x => x.Id == alerta.MonitorId);
                    if (monitor != null && monitor.State != MonitorState.Disconected)
                    {
                        using var scope = serviceProvider.CreateScope();
                        await scope.ServiceProvider.GetService<IHubContext<THub>>()!.Clients.Client(monitor.ConnectionId).SendAsync("OnChangeAlerta", alerta);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Get().Error("UpdatePosition", ex);
            }
        }
    }

    /// <summary>
    /// Estados del usuario monitor
    /// </summary>
    public enum MonitorState
    {
        /// <summary>
        /// Sin estado
        /// </summary>
        None,
        /// <summary>
        /// Esperando recibr una alerta
        /// </summary>
        Waiting,
        /// <summary>
        /// Tiene asignado una alerta
        /// </summary>
        Asigned,
        /// <summary>
        /// En estado de pausa
        /// </summary>
        Pause,
        /// <summary>
        /// Desconectado del sistema
        /// </summary>
        Disconected,
    }

    /// <summary>
    /// Objeto de la alerta para el control del mismo
    /// </summary>
    public class AlertaContent : Alerta
    {
        /// <summary>
        /// Se agrego para el control el campo TimeOut
        /// </summary>
        public long TimeOut { get; set; } = 0;
        public long TimeSendSMS { get; set; } = 0;

        public static AlertaContent Create(Alerta data)
        {
            AlertaContent content = new AlertaContent();
            content.Id = data.Id;
            content.TenantId = data.TenantId;
            content.AfiliadoId = data.AfiliadoId;
            content.AfiliadoNombreCompleto = data.AfiliadoNombreCompleto;
            content.AfiliadoTelefonoContacto = data.AfiliadoTelefonoContacto;
            content.AfiliadoUbicacion = data.AfiliadoUbicacion;
            content.AfiliadoCodigoPostal = data.AfiliadoCodigoPostal;

            content.AfiliadoEdad = data.AfiliadoEdad;
            content.AfiliadoSexo = data.AfiliadoSexo;
            content.AfiliadoNosocomio = data.AfiliadoNosocomio;

            content.AlarmaSonora = data.AlarmaSonora;

            content.Bitacora = data.Bitacora;

            content.Cerrado = data.Cerrado;

            content.ConfirmaAsignacion = data.ConfirmaAsignacion;
            content.MonitorId = data.MonitorId;

            content.Position = data.Position;

            content.Estado = data.Estado;
            content.Creado = data.Creado;
            content.Modificacion = data.Modificacion;

            return content;
        }
    }

    /// <summary>
    /// Objeto que maneja el estado del monitor
    /// </summary>
    public class Monitor
    {
        public Monitor(string id, string connectionId, MonitorState state = MonitorState.Waiting)
        {
            Id = id;
            ConnectionId = connectionId;
            ChangeState(state);
        }

        public string Id { get; private set; }
        public string ConnectionId { get; private set; }
        public MonitorState State { get; private set; } = MonitorState.Waiting;
        public long Ticks { get; private set; } = DateTime.UtcNow.Ticks;
        public long TimeOut { get; private set; } = 0;
        private List<string>? CodigoPostal { get; set; }

        public List<string>? GetCodigoPostal() { return CodigoPostal; }
        public void SetCodigoPostal(List<string>? codigoPostal) { CodigoPostal = codigoPostal; }

        public void SetTimeOut(long timeOut)
        {
            TimeOut = timeOut;
        }

        public void ChangeState(MonitorState state)
        {
            if (State != state)
                Ticks = DateTime.UtcNow.Ticks;
            State = state;
        }

        public void SetConnectionId(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}