using GQ.Architecture.DDD.Domain;
using GQ.Architecture.DDD.Domain.Auth;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Architecture.DDD.Domain.Exceptions;
using GQ.Data.Abstractions.Paging;
using UCare.Domain.Comunicaciones;
using UCare.Domain.Users;
using UCare.Shared.Application;
using UCare.Shared.Domain;
using UCare.Shared.Domain.Auth;

namespace UCare.Application.Comunicaciones
{
    public class ComunicacionApp : ApplicationBase
    {
        private readonly IComunicacionRepository repository;
        private readonly IUsuarioAfiliadoRepository afiliadoRepository;

        public ComunicacionApp(IComunicacionRepository repository, IUsuarioAfiliadoRepository afiliadoRepository, IAuthUserRepository authUser, IEventBus eventBus, ICacheService cacheService) : base(authUser, eventBus, cacheService)
        {
            this.repository = repository;
            this.afiliadoRepository = afiliadoRepository;
        }

        public Task<Shared.Infrastructure.IPaging> Get(Shared.Infrastructure.IPaging paging)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            if (!string.IsNullOrWhiteSpace(user.TenantId))
                paging.Filter.Add(new PagingFilter { Property = "TenantId", Condition = "=", Value = user.TenantId });

            paging.Order.Add(new PagingOrder { Property = "FechaEnvio", Direction = "+" });

            return repository.Get(paging);
        }

        public Task<Comunicacion> GetById(string id)
        {
            return repository.GetById(id);
        }

        public async Task<Comunicacion> Save(Comunicacion model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            Modificacion modificacion = new Modificacion { UsuarioId = user.Id, Modificado = DateTime.UtcNow };

            model.FechaEnvio = model.FechaEnvio ?? DateTime.UtcNow.AddSeconds(-1);
            model.Enviado = false;
            model.DetalleEnvio = "";

            if (string.IsNullOrEmpty(model.Id))
            {   //Agregamos un usuario nuevo
                model.TenantId = user.TenantId ?? model.TenantId;

                //Validamos los campos requeridos
                if (model.Validate())
                {
                    model = Comunicacion.CreateComunicacion(model, modificacion);
                    model = await repository.Insert(model);
                }
                else
                {
                    throw new ValidationException(model.ValidateDetails());
                }
            }
            else
            {   // Editamos un usuario existente

                var entity = await repository.GetById(model.Id);

                if (entity.Enviado)
                {
                    throw new Exception("No se puede modificar una comunicacion que ya ha sido enviada");
                }

                entity.ModificarComunicacion(model, modificacion);

                //Validamos los campos requeridos
                if (entity.Validate())
                {
                    await repository.Update(entity);
                }
                else
                {
                    throw new ValidationException(entity.ValidateDetails());
                }
                model = entity;
            }

            await eventBus.Publish(model.PullDomainEvents());

            return model;
        }

        public async Task Delete(string id)
        {
            var entity = await repository.GetById(id);
            entity.Estado = UCare.Shared.Domain.ValueObjects.Estados.Borrado;
            await repository.Update(entity);
        }

        public Task<Shared.Infrastructure.IPaging> GetAfiliados(Shared.Infrastructure.IPaging paging)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            if (user != null && !string.IsNullOrWhiteSpace(user.TenantId))
                paging.Filter.Add(new PagingFilter { Property = "TenantId", Condition = "=", Value = user.TenantId });
            return afiliadoRepository.Get(paging);
        }

        public Task<Shared.Infrastructure.IPaging> GetAllAfiliados(Shared.Infrastructure.IPaging paging)
        {
            paging.SetMaxPageSize(int.MaxValue - 1);
            paging.PageSize = int.MaxValue - 1;
            return GetAfiliados(paging);
        }

        public async Task<List<Comunicacion>> GetPendientes()
        {
            return await repository.GetPandientes();
        }

        public async Task<string> AfiliadoAddComunicacion(UsuarioAfiliado afilado, Comunicacion comunicacion)
        {
            ComunicacionAfiliado comunicado = afilado.CreateComunicado(comunicacion);
            var comunicadoId = await afiliadoRepository.ComunicadoAdd(afilado.Id!, comunicado);

            await eventBus.Publish(afilado.PullDomainEvents());

            return comunicadoId;
        }

        public Task<bool> UpadeteEstado(Comunicacion comunicacion, List<ComunicacionEnvio> envios)
        {
            return repository.UpadeteEstado(comunicacion, envios);
        }
    }
}
