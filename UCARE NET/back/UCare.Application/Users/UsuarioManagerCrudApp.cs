using GQ.Architecture.DDD.Domain;
using GQ.Architecture.DDD.Domain.Auth;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Architecture.DDD.Domain.Exceptions;
using GQ.Data.Abstractions.Paging;
using UCare.Application.Alertas;
using UCare.Domain.Users;
using UCare.Shared.Application;
using UCare.Shared.Domain;
using UCare.Shared.Domain.Auth;
using UCare.Shared.Domain.ValueObjects;

namespace UCare.Application.Users
{
    public class UsuarioManagerCrudApp : ApplicationBase
    {
        private readonly IUsuarioManagerRepository repository;
        private readonly AlertasByServiceApp appAlert;
        public UsuarioManagerCrudApp(AlertasByServiceApp appAlert, IUsuarioManagerRepository repository, IAuthUserRepository authUser, IEventBus eventBus, ICacheService cacheService) : base(authUser, eventBus, cacheService)
        {
            this.repository = repository;
            this.appAlert = appAlert;
        }

        public Task<List<UsuarioManager>> GetAllUserManager()
        {
            return repository.GetAllUserManager();
        }

        public Task<Shared.Infrastructure.IPaging> Get(Shared.Infrastructure.IPaging paging)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var rol = UsuarioManager.GetRolFilter(user.Rol);
            if (!string.IsNullOrWhiteSpace(user.TenantId))
                paging.Filter.Add(new PagingFilter { Property = "TenantId", Condition = "=", Value = user.TenantId });
            paging.Filter.Add(new PagingFilter { Property = "Rol", Condition = "in", Value = rol });

            if (paging.Order.Count == 0)
            {
                paging.Order.Add(new PagingOrder { Property = "UsuarioNombre", Direction = "+" });
            }
            return repository.Get(paging);
        }

        public Task<UsuarioManager> GetById(string id)
        {
            return repository.GetById(id);
        }

        public async Task<UsuarioManager> Save(UsuarioManager model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var roles = UsuarioManager.GetRolAdd(user.Rol);
            model.Email = model.Email.ToLower();
            Modificacion modificacion = new Modificacion { UsuarioId = user.Id, Modificado = DateTime.UtcNow };
            model.Rol = roles.Any(x => x.Equals(model.Rol)) ? model.Rol : (roles.Length == 1 ? roles.First() : "");
            if (string.IsNullOrEmpty(model.Id))
            {   //Agregamos un usuario nuevo

                model.TenantId = user.TenantId ?? model.TenantId;

                //Validamos los campos requeridos
                if (model.Validate())
                {
                    var existEmail = await repository.GetByUserEmail(model.Email);

                    if (existEmail != null)
                    {
                        throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Email ya utilizado", new List<string> { "Email" }) });
                    }

                    model.UsuarioId = user.Id;
                    model = UsuarioManager.CreateUsuario(model, modificacion);
                    model.CambiarPassword(model.Password!);
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
                if (roles.Any(x => x == entity.Rol))
                {
                    entity.ModificarUsuario(model, modificacion);

                    //Validamos los campos requeridos
                    if (entity.Validate())
                    {
                        if (!Estados.AllEstadosUsuario.Contains(entity.Estado))
                        {
                            throw new Exception("Estado incorrecto");
                        }

                        //Chequeamos si el usuario tiene subordinados para poder hacer el cambio de estado
                        if (entity.Estado != Estados.Activo && (await GetUsersByUserId(entity.Id!)).Count > 0)
                        {
                            throw new Exception("No puede borrar o desactivar un usuario si este tiene subordinados");
                        }

                        //Chequeamos que la persona que hace el cambio sea la persona que tiene como subordinado al usuario
                        if (entity.UsuarioId != user.Id)
                        {
                            throw new Exception("No tiene permisos para modificar a este usuario");
                        }

                        var existEmail = await repository.GetByUserEmail(model.Email);

                        if (existEmail != null && existEmail.Id != entity.Id)
                        {
                            throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Email ya utilizado", new List<string> { "Email" }) });
                        }

                        await repository.Update(entity);
                    }
                    else
                    {
                        throw new ValidationException(entity.ValidateDetails());
                    }
                    model = entity;
                }
            }
            return model;
        }

        public async Task<bool> GetReplaceById(string id, string idTo)
        {
            var list = await GetUsersByUserId(id);
            foreach (var item in list)
            {
                item.UsuarioId = idTo;
                await repository.UpdateUsuarioId(item);
            }
            return true;
        }

        public async Task<List<UsuarioManager>> GetUsersByIdRol(string id)
        {
            return await repository.GetUsersByIdRol(id);
        }

        public async Task<List<UsuarioManager>> GetUsersByUserId(string id)
        {
            return await repository.GetUsersByUserId(id);
        }

        public async Task Delete(string id)
        {
            var entity = await repository.GetById(id);
            entity.Estado = UCare.Shared.Domain.ValueObjects.Estados.Borrado;
            await repository.Update(entity);
        }

        public async Task CreateUsuarioPropietario()
        {
            var usuarios = await repository.GetByRol(Roles.Propietario);
            if (usuarios.Count == 0)
            {
                var usuarioPropietario = UsuarioManager.CreateUsuarioPropietario();
                _ = await repository.Insert(usuarioPropietario);
            }
        }

        public Task<List<string>?> GetCodigosPostales()
        {
            var user = authUser.GetAuthUser<AuthUser>();
            return appAlert.GetCodigosPostales(user.Id);
        }

        
    }
}
