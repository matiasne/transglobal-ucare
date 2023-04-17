using GQ.Architecture.DDD.Domain;
using GQ.Architecture.DDD.Domain.Auth;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Data.Abstractions.Paging;
using UCare.Domain.Config;
using UCare.Shared.Application;
using UCare.Shared.Domain;
using UCare.Shared.Domain.Auth;
using UCare.Shared.Infrastructure.AlertasService;

namespace UCare.Application.Users
{
    public class ConfigCrudApp : ApplicationBase
    {
        private readonly IConfigRepository repository;
        private readonly IAlertasService alertasService;
        public ConfigCrudApp(IAlertasService alertasService, IConfigRepository repository, IAuthUserRepository authUser, IEventBus eventBus, ICacheService cacheService) : base(authUser, eventBus, cacheService)
        {
            this.repository = repository;
            this.alertasService = alertasService;
        }

        public Task<Shared.Infrastructure.IPaging> Get(Shared.Infrastructure.IPaging paging)
        {
            var user = authUser.GetAuthUser<AuthUser>();

            if (!string.IsNullOrWhiteSpace(user.TenantId))
                paging.Filter.Add(new PagingFilter { Property = "TenantId", Condition = "=", Value = user.TenantId });

            return repository.Get(paging);
        }

        public Task<Config> GetById(string id)
        {
            return repository.GetById(id);
        }

        public async Task<Config> Save(Config model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            Modificacion modificacion = new Modificacion { UsuarioId = user.Id, Modificado = DateTime.UtcNow };
            if (string.IsNullOrWhiteSpace(model.Id))
            {
                model.Creado = modificacion;
                model.Modificacion = modificacion;
                await repository.Insert(model);
            }
            else
            {
                model.Modificacion = modificacion;
                await repository.Update(model);
            }
            alertasService.ChangeConfig();
            return await GetById("");
        }

        public async Task Delete(string id)
        {
            var entity = await repository.GetById(id);
            entity.Estado = UCare.Shared.Domain.ValueObjects.Estados.Borrado;
            await repository.Update(entity);
        }
    }
}
