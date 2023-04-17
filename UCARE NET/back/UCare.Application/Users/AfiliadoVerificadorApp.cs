using GQ.Architecture.DDD.Domain;
using GQ.Architecture.DDD.Domain.Auth;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Architecture.DDD.Domain.Exceptions;
using GQ.Data.Abstractions.Paging;
using UCare.Application.Alertas;
using UCare.Application.Users.Exceptions;
using UCare.Domain.Config;
using UCare.Domain.Users;
using UCare.Shared.Application;
using UCare.Shared.Domain.Auth;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure.PhoneNumber;

namespace UCare.Application.Users
{
    public class AfiliadoVerificadorApp : ApplicationBase
    {
        private readonly IUsuarioAfiliadoRepository repository;
        private readonly IUsuarioManagerRepository usuarioManager;
        private readonly IValidatePhoneNumber validatePhone;
        private readonly AlertasByServiceApp app;
        private readonly IConfigRepository configRepository;

        public AfiliadoVerificadorApp(AlertasByServiceApp app, IConfigRepository configRepository, IValidatePhoneNumber validatePhone, IUsuarioAfiliadoRepository repository, IUsuarioManagerRepository usuarioManager, IAuthUserRepository authUser, IEventBus eventBus, ICacheService cacheService) : base(authUser, eventBus, cacheService)
        {
            this.repository = repository;
            this.usuarioManager = usuarioManager;
            this.validatePhone = validatePhone;
            this.configRepository = configRepository;
            this.app = app;
        }

        public async Task<Shared.Infrastructure.IPaging> Get(Shared.Infrastructure.IPaging paging)
        {
            var pageSize = paging.PageSize;
            paging.SetMaxPageSize(int.MaxValue);
            paging.PageSize = paging.GetMaxPageSize();
            var user = authUser.GetAuthUser<AuthUser>();
            if (!string.IsNullOrWhiteSpace(user.TenantId))
                paging.Filter.Add(new PagingFilter { Property = "TenantId", Condition = "=", Value = user.TenantId });

            if (paging.Order.Count == 0)
            {
                paging.Order.Add(new PagingOrder { Property = "Creado.Modificado", Direction = "+" });
            }

            var result = await repository.Get(paging);

            var cp = await app.GetCodigosPostales(user.Id);

            var list = result.Data as List<UsuarioAfiliado>;

            if (list != null && list.Count > 0)
            {
                list = list.Where(x => x.Estado != Estados.SinVerificar).ToList();

                var newList = new List<UsuarioAfiliado>();

                foreach (var item in list)
                {
                    if (cp == null || cp.Any(x => x == item.Direccion.CodigoPostal))
                    {
                        newList.Add(item);
                    }
                }

                result.PageSize = pageSize;
                result.RecordCount = newList.Count;

                result.PageCount = newList.Count / result.PageSize;
                if (newList.Count % result.PageSize > 0) result.PageCount++;

                result.Data = newList.Skip(((paging?.PageIndex) - 1) * result.PageSize ?? 0).Take(result.PageSize ?? 0);
            }

            return result;
        }

        public async Task<UsuarioAfiliado> CambarEstado(UsuarioAfiliado model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(model.Id);

            if (new string[] { Estados.Activo, Estados.Desactivo }.Contains(model.Estado))
            {

                if (model.Estado == Estados.Activo)
                {
                    var config = await configRepository.GetById(null);
                    if ((await repository.GetCountActivos()) >= config.UsuarioActivosMaximos)
                    {
                        throw new Exception($"Ya ha llegado al maximo de usuarios ({config.UsuarioActivosMaximos}). No puede activar mas usuarios");
                    }
                }

                entity.CambarEstado(model, user.Id);
                _ = await repository.UpdateEstado(entity);
                await eventBus.Publish(entity.PullDomainEvents());

                return entity;
            }
            else
            {
                throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Estado incorrecto solo puede seleccionar Activo o Inactivo", new List<string> { "Estado" }) });
            }
        }

        public async Task<UsuarioAfiliado> GetById(string id)
        {
            return await repository.GetById(id);
        }

        public async Task<UsuarioAfiliado> Save(UsuarioAfiliado model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(model.Id);
            if (entity != null)
            {
                var estadoAnt = entity.Estado;


                entity.Direccion = model.Direccion;
                entity.FechaNacimiento = model.FechaNacimiento;
                entity.Lenguaje = model.Lenguaje;
                entity.Nosocomio = model.Nosocomio;
                entity.NumeroIdentidad = model.NumeroIdentidad;
                entity.Sexo = model.Sexo.ToUpper();
                entity.UsuarioNombre = model.UsuarioNombre;
                entity.CambarEstado(model, user.Id);

                if (!Estados.AllEstadosUsuario.Contains(entity.Estado))
                {
                    throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Estado incorrecto", new List<string> { "Estado" }) });
                }

                if (!Generos.All.Contains(entity.Sexo))
                {
                    throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Valor del campo sexo incorrecto", new List<string> { "Sexo" }) });
                }

                model = entity;

                if (estadoAnt == Estados.Revision || new string[] { Estados.Activo, Estados.Desactivo }.Contains(model.Estado))
                {
                    if (model.Validate())
                    {
                        var entityNI = await repository.GetByUserNumerodeIdentidad(model.NumeroIdentidad);

                        if (entityNI != null && entityNI.Id != model.Id)
                        {
                            throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Número de identidad ya existe en otro afiliado", new List<string> { "NumeroIdentidad" }) });
                        }

                        if (estadoAnt != Estados.Activo && model.Estado == Estados.Activo)
                        {
                            var config = await configRepository.GetById(null);
                            if ((await repository.GetCountActivos()) >= config.UsuarioActivosMaximos)
                            {
                                throw new Exception($"Ya ha llegado al maximo de usuarios ({config.UsuarioActivosMaximos}). No puede activar mas usuarios");
                            }
                        }

                        var result = await repository.Update(model);

                        if (result)
                        {
                            await eventBus.Publish(model.PullDomainEvents());
                            return model;
                        }
                    }
                    else
                    {
                        throw new ValidationException(model.ValidateDetails());
                    }
                }
                else
                {
                    throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Estado incorrecto solo puede seleccionar Activo o Inactivo", new List<string> { "Estado" }) });
                }
            }
            return null;
        }

        public async Task<bool> ExistNumeroIdentidad(string numeroIdentidad, string id)
        {
            var entity = (await repository.GetByUserNumerodeIdentidad(numeroIdentidad));
            return entity != null && entity.Id != id;
        }

        public async Task<bool> ExistEmail(string email, string id)
        {
            var entity = (await usuarioManager.GetByUserEmail(email));
            return entity != null && entity.Id != id;
        }

        public async Task<bool> ExistPhoneNumber(string phoneNumber, string countryCode, string id)
        {
            countryCode = countryCode.StartsWith("+") ? countryCode : $"+{countryCode}";
            phoneNumber = phoneNumber.StartsWith("+") ? phoneNumber : $"+{phoneNumber}";

            if (validatePhone.Validate(phoneNumber, countryCode))
            {
                var entity = (await repository.GetByUserPhoneNumber(phoneNumber));
                return entity != null && entity.Id != id;
            }
            throw new NumeroTelefonoException();
        }
    }
}
