using GQ.Architecture.DDD.Domain;
using GQ.Architecture.DDD.Domain.Auth;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Architecture.DDD.Domain.Exceptions;
using GQ.Log;
using UCare.Domain.Alertas;
using UCare.Domain.Users;
using UCare.Shared.Application;
using UCare.Shared.Domain.Auth;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure.Locations;

namespace UCare.Application.Alertas
{
    public class AlertaApp : ApplicationBase
    {
        private readonly IAlertaRepository repository;
        private readonly IUsuarioAfiliadoRepository repositoryAfiliado;
        private readonly ILocationRepository location;

        public AlertaApp(IAlertaRepository repository, IUsuarioAfiliadoRepository repositoryAfiliado, ILocationRepository location, IAuthUserRepository authUser, IEventBus eventBus, ICacheService cacheService) : base(authUser, eventBus, cacheService)
        {
            this.repository = repository;
            this.repositoryAfiliado = repositoryAfiliado;
            this.location = location;
        }

        public async Task<bool> CambiarEstadoEmergencia(Alerta model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(model.Id);
            entity.CambairEstado(UCare.Shared.Domain.ValueObjects.Estados.Emergencia, user);
            await repository.UpdateState(entity);
            await eventBus.Publish(model.PullDomainEvents());
            return true;
        }

        public async Task<bool> CambiarEstadoUrgencia(Alerta model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(model.Id);
            entity.CambairEstado(UCare.Shared.Domain.ValueObjects.Estados.Urgencia, user);
            await repository.UpdateState(entity);
            await eventBus.Publish(model.PullDomainEvents());
            return true;
        }

        public async Task<bool> CambiarEstadoFalsaAlarma(Alerta model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(model.Id);
            entity.CambairEstado(UCare.Shared.Domain.ValueObjects.Estados.FalsaAlarma, user);
            await repository.UpdateState(entity);
            await eventBus.Publish(model.PullDomainEvents());
            return true;
        }

        public async Task<bool> GuardarBitacora(Alerta model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(model.Id);
            entity.CambairEstado(UCare.Shared.Domain.ValueObjects.Estados.Emergencia, user);
            await repository.UpdateBitacora(entity);
            await eventBus.Publish(model.PullDomainEvents());
            return true;
        }

        public async Task<bool> GuardarUbicacion(Alerta model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(model.Id);
            entity.CambairEstado(UCare.Shared.Domain.ValueObjects.Estados.Emergencia, user);
            await repository.UpdateUbicacione(entity);
            await eventBus.Publish(model.PullDomainEvents());
            return true;
        }

        public async Task<bool> UpdateAlert(string id, GeoPosition geoPosition)
        {
            var entity = await repository.GetById(id);
            if (entity != null/* && entity.AfiliadoId == user.Id*/ && !(entity.Cerrado ?? false))
            {
                entity.CambiarPosicion(geoPosition, new AuthUser { Id = entity.AfiliadoId });
                await repository.UpdatePosition(entity);
                await eventBus.Publish(entity.PullDomainEvents());
                return true;
            }
            else
            {
                Log.Get().Warn("No se pudo actualizar la alerta ");
            }
            return false;
        }

        public async Task<Alerta?> GetAlertByUser()
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetAlertByUserId(user.Id);
            return entity;
        }

        public async Task<Alerta?> GetAlertByUser(string id)
        {
            var entity = await repository.GetAlertByUserId(id);
            return entity;
        }

        public Task<bool> SendAlert(GeoPosition position)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            return SendAlert(user.Id, position);
        }

        public async Task<bool> SendAlert(string? id, GeoPosition position)
        {
            var entity = await repositoryAfiliado.GetById(id);
            ///COrreccion segun tk #34865
            if (!(new string[] { Estados.Activo, Estados.Revision }.Contains(entity.Estado)) || entity.Rol != Roles.Afiliado)
                return false;

            var alerta = await GetAlertByUser(id!);

            if (alerta == null)
            {
                var locations = await location.GetDirectionByPos(position!.Lat.Value, position!.Lon.Value);

                alerta = Alerta.Create(entity, position, locations);

                if (alerta.Validate())
                {
                    alerta = await repository.Insert(alerta);
                    alerta.CreateEvent();
                    await eventBus.Publish(alerta.PullDomainEvents());
                    return true;
                }
                throw new ValidationException(entity.ValidateDetails());
            }
            return true;
        }

    }
}
