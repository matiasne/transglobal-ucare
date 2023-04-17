using GQ.Architecture.DDD.Application;
using GQ.Architecture.DDD.Domain.Bus.Event;
using UCare.Domain.Alertas;
using UCare.Domain.Users;

namespace UCare.Application.Alertas
{
    public class AlertasByServiceApp : IApplication
    {
        private readonly IAlertaRepository repository;
        private readonly IUsuarioManagerRepository managerRepository;
        private readonly IEventBus eventBus;

        public string Name => typeof(AlertasByServiceApp).FullName;

        public AlertasByServiceApp(IAlertaRepository repository, IUsuarioManagerRepository managerRepository, IEventBus eventBus)
        {
            this.repository = repository;
            this.managerRepository = managerRepository;
            this.eventBus = eventBus;
        }

        public Task<List<Alerta>> GetPendientes()
        {
            return repository.GetPendientes();
        }

        public Task<List<Alerta>> GetPendientesAsignados()
        {
            return repository.GetPendientesAsignados();
        }

        public Task<List<Alerta>> GetPendientesAsignados(string id)
        {
            return repository.GetPendientesAsignados(id);
        }

        public Task<UsuarioManager> GetMonitorById(string id)
        {
            return managerRepository.GetById(id);
        }

        public Task<Alerta> GetAlertaById(string id)
        {
            return repository.GetById(id);
        }

        public async Task<bool> Asignar(Alerta model, string monitorId)
        {
            if (string.IsNullOrWhiteSpace(monitorId))
            {
                await managerRepository.RegistrarHistorial(model.MonitorId, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.DESASIGNAR, model.Id, model.AfiliadoId));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.MonitorId))
                {
                    await managerRepository.RegistrarHistorial(model.MonitorId, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.DESASIGNAR, model.Id, model.AfiliadoId));
                }
                await managerRepository.RegistrarHistorial(monitorId, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.ASIGNAR, model.Id, model.AfiliadoId));
            }
            model.AsignarMonitor(monitorId, null);
            var result = await repository.UpdateAsignare(model);
            if (result)
            {
                await eventBus.Publish(model.PullDomainEvents());
            }
            return result;
        }


        public async Task<bool> ConfirmarAsignacion(Alerta model, string monitorId)
        {
            await managerRepository.RegistrarHistorial(model.MonitorId, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.CONFIRMAR, model.Id, model.AfiliadoId));
            model.ConfirmarAsignacion(monitorId);
            var result = await repository.UpdateConfirmarAsignacion(model);
            if (result)
            {
                await eventBus.Publish(model.PullDomainEvents());
            }
            return result;
        }

        public async Task<Alerta?> CambiarEstado(string id, string alertaId, string estado)
        {
            var model = await repository.GetById(alertaId);
            model.CambairEstado(estado, new Shared.Domain.Auth.AuthUser { Id = id });

            await managerRepository.RegistrarHistorial(model.MonitorId, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.CAMBIAR_ESTADO, model.Id, model.AfiliadoId));
            var result = await repository.UpdateState(model);
            if (result)
            {
                await eventBus.Publish(model.PullDomainEvents());
                return model;
            }
            return null;
        }

        public async Task<Alerta?> ActivarDesactivarAlarma(string id, string alertaId, bool alarma)
        {
            var model = await repository.GetById(alertaId);
            model.CambairEstadoAlarma(alarma, new Shared.Domain.Auth.AuthUser { Id = id });

            await managerRepository.RegistrarHistorial(model.MonitorId, UsuarioManagerHistorial.Create(alarma ? UsuarioManagerHistorialAcciones.ACTIVAR_ALARMA : UsuarioManagerHistorialAcciones.DESACTIVAR_ALARMA, model.Id, model.AfiliadoId));
            var result = await repository.UpdateAlarma(model);
            if (result)
            {
                await eventBus.Publish(model.PullDomainEvents());
                return model;
            }
            return null;
        }

        public async Task<Alerta?> FinalizarAsistencia(string id, string alertaId, string bitacora)
        {
            var model = await repository.GetById(alertaId);
            model.FinalizarAsistencia(bitacora, new Shared.Domain.Auth.AuthUser { Id = id });
            await managerRepository.RegistrarHistorial(model.MonitorId, UsuarioManagerHistorial.Create(UsuarioManagerHistorialAcciones.FINALIZAR, model.Id, model.AfiliadoId));
            var result = await repository.UpdateFinalizacion(model);
            if (result)
            {
                await eventBus.Publish(model.PullDomainEvents());
                return model;
            }
            return null;
        }

        public async Task RegistrarHistorialMonitor(string? id, UsuarioManagerHistorial usuarioManagerHistorial)
        {
            await managerRepository.RegistrarHistorial(id!, usuarioManagerHistorial);
        }

        public async Task<List<string>?> GetCodigosPostales(string id)
        {
            var user = await managerRepository.GetById(id);
            if (user != null && (user.CodigoPostal == null || user.CodigoPostal.Count == 0) && !string.IsNullOrWhiteSpace(user.UsuarioId))
            {
                return await GetCodigosPostales(user.UsuarioId);
            }
            return (user != null && (user.CodigoPostal == null || user.CodigoPostal.Count == 0)) ? null : user?.CodigoPostal;
        }
    }
}
