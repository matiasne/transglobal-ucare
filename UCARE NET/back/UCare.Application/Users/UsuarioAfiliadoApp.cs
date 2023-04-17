using GQ.Architecture.DDD.Domain;
using GQ.Architecture.DDD.Domain.Auth;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Architecture.DDD.Domain.Exceptions;
using GQ.Data.Abstractions.Paging;
using System.Reflection;
using UCare.Application.Users.Exceptions;
using UCare.Domain.Comunicaciones;
using UCare.Domain.Users;
using UCare.Shared.Application;
using UCare.Shared.Domain.Auth;
using UCare.Shared.Domain.Locations;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure.Locations;
using UCare.Shared.Infrastructure.Notifications;
using UCare.Shared.Infrastructure.PhoneNumber;

namespace UCare.Application.Users
{
    public class UsuarioAfiliadoApp : ApplicationBase
    {
        private readonly IUsuarioAfiliadoRepository repository;
        private readonly ILocationRepository location;
        private readonly IValidatePhoneNumber validatePhone;
        private readonly IComunicacionRepository comunicacionRepository;
        private readonly INotificationRepository notificationRepository;

        public UsuarioAfiliadoApp(INotificationRepository notificationRepository, IComunicacionRepository comunicacionRepository, IUsuarioAfiliadoRepository repository, ILocationRepository location, IValidatePhoneNumber validatePhone, IAuthUserRepository authUser, IEventBus eventBus, ICacheService cacheService) : base(authUser, eventBus, cacheService)
        {
            this.repository = repository;
            this.location = location;
            this.validatePhone = validatePhone;
            this.comunicacionRepository = comunicacionRepository;
            this.notificationRepository = notificationRepository;
        }

        public async Task<Shared.Infrastructure.IPaging> Get(Shared.Infrastructure.IPaging paging)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            if (!string.IsNullOrWhiteSpace(user.TenantId))
                paging.Filter.Add(new PagingFilter { Property = "TenantId", Condition = "=", Value = user.TenantId });

            return await repository.Get(paging);
        }

        public async Task<UsuarioAfiliado> Registrar(UsuarioAfiliado model)
        {
            model.Id = null;
            model.Rol = UCare.Shared.Domain.ValueObjects.Roles.Afiliado;
            model.Email = model.Email.ToLower();
            model.VerificaEmail = new Verificacion();
            model.VerificaTelefono = new Verificacion();
            model.Estado = UCare.Shared.Domain.ValueObjects.Estados.SinVerificar;
            model.Sexo = model.Sexo.ToUpper();

            model.CodigoPais = model.CodigoPais.StartsWith("+") ? model.CodigoPais : $"+{model.CodigoPais}";
            model.Celular = model.Celular.StartsWith("+") ? model.Celular : $"+{model.Celular}";

            if (model.Validate())
            {
                if (model.Direccion.Validate())
                {
                    if (model.FechaNacimiento.Ticks >= DateTime.UtcNow.Ticks)
                    {
                        throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("La Fecha de nacimiento no puede ser mayor que la fecha actual", new List<string> { "FechaNacimiento" }) });
                    }
                    if (!validatePhone.Validate(model.Celular, model.CodigoPais))
                    {
                        throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Número de telefono invalido", new List<string> { "Celular" }) });
                    }
                    if (await ExistPhoneNumber(model.Celular, model.CodigoPais))
                    {
                        throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Número de celular ya utilizado", new List<string> { "Celular" }) });
                    }
                    if (await ExistEmail(model.Email))
                    {
                        throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Email ya utilizado", new List<string> { "Email" }) });
                    }
                    if (await ExistNumeroIdentidad(model.NumeroIdentidad))
                    {
                        throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Número de identidad ya existe", new List<string> { "NumeroIdentidad" }) });
                    }

                    model.CambiarEmail(model);
                    model.CambiarPassword(model.Password!);
                    model.CambiarTelefono(model.Celular, model.CodigoPais);

                    model = await repository.Insert(model);

                    //retrazo porque la app mobile no cambio de estado y el mensaje llega antes cque cambie de estado
                    Task.Delay(5000).ContinueWith((t) =>
                    {
                        eventBus.Publish(model.PullDomainEvents()).Wait();
                    });

                    return model;
                }
                else
                {
                    throw new ValidationException(model.Direccion.ValidateDetails());
                }
            }
            else
            {
                throw new ValidationException(model.ValidateDetails());
            }
        }

        public async Task<UsuarioAfiliado> GetAfiliado()
        {
            return await repository.GetById(authUser.GetAuthUser<AuthUser>().Id);
        }

        public async Task<UsuarioAfiliado> CambiarDatosPersonales(UsuarioAfiliado model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(user.Id);
            if (entity.ValidatePassword(model.Password!))
            {
                if (!validatePhone.Validate(model.Celular, model.CodigoPais))
                {
                    throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Número de telefono invalido", new List<string> { "Celular" }) });
                }

                entity.CambiarDatosPersonales(model);

                if (!string.IsNullOrEmpty(model.Email) && model.Email != entity.Email)
                    entity.CambiarEmail(model);

                if (!string.IsNullOrEmpty(model.Celular) && !string.IsNullOrEmpty(model.CodigoPais) &&
                    model.CodigoPais.Replace("+", "") != entity.CodigoPais?.Replace("+", "") || model.Celular.Replace("+", "") != entity.Celular?.Replace("+", ""))
                {
                    if (await ExistPhoneNumber(model.Celular, model.CodigoPais, user.Id))
                    {
                        throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Número de celular ya utilizado", new List<string> { "Celular" }) });
                    }

                    entity.CambiarTelefono(model.Celular, model.CodigoPais);
                }

                if (entity.Validate())
                {
                    if (entity.Direccion.Validate())
                    {
                        _ = await repository.UpdateDatosPersonales(entity);
                        _ = await repository.UpdateCelular(entity);

                        await eventBus.Publish(entity.PullDomainEvents());
                    }
                    else
                    {
                        throw new ValidationException(entity.Direccion.ValidateDetails());
                    }
                }
                else
                {
                    throw new ValidationException(entity.ValidateDetails());
                }
            }
            else
                throw new PasswordIncorrectoException();
            return entity;
        }

        public async Task<UsuarioAfiliado> CambiarContactos(UsuarioAfiliado model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(user.Id);
            if (entity.ValidatePassword(model.Password))
            {
                entity.CambiarContactos(model);

                _ = await repository.UpdateContactos(entity);
                await eventBus.Publish(entity.PullDomainEvents());
            }
            else
                throw new PasswordIncorrectoException();
            return entity;
        }

        public async Task<UsuarioAfiliado> CambiarPatologias(UsuarioAfiliado model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(user.Id);

            if (entity.ValidatePassword(model.Password))
            {
                entity.CambiarPatologias(model);

                _ = await repository.UpdatePatologias(entity);
                await eventBus.Publish(entity.PullDomainEvents());
            }
            else
                throw new PasswordIncorrectoException();

            return entity;
        }

        public async Task<UsuarioAfiliado> CambiarCelular(UsuarioAfiliado model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(user.Id);
            if (entity.ValidatePassword(model.Password!))
            {
                if (await ExistPhoneNumber(model.Celular, model.CodigoPais, user.Id))
                {
                    throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Número de celular ya utilizado", new List<string> { "Celular" }) });
                }
                entity.CambiarTelefono(model.Celular, model.CodigoPais);
                _ = await repository.UpdateCelular(entity);
                await eventBus.Publish(entity.PullDomainEvents());
            }
            else
                throw new PasswordIncorrectoException();
            return entity;
        }

        public async Task<UsuarioAfiliado> CambiarNumeroIdentidad(UsuarioAfiliado model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(user.Id);
            if (entity.ValidatePassword(model.Password!))
            {
                entity.CambiarNumeroIdentidad(model.NumeroIdentidad);
                _ = await repository.UpdateNumeroIdentidad(entity);
                await eventBus.Publish(entity.PullDomainEvents());
            }
            else
                throw new PasswordIncorrectoException();
            return entity;
        }

        public async Task<bool> VerifyCodeBySms(string codigo)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(user.Id);
            if (entity != null && entity.VerificaTelefono.CodigoVerificacion == codigo)
            {
                entity.VerificaTelefono.Verificado = true;
                if (entity.Estado == UCare.Shared.Domain.ValueObjects.Estados.SinVerificar)
                {
                    entity.Estado = UCare.Shared.Domain.ValueObjects.Estados.Revision;
                    _ = await repository.UpdateEstado(entity);
                }
                _ = await repository.UpdateCelular(entity);

                await eventBus.Publish(entity.PullDomainEvents());
                return true;
            }
            return false;
        }

        public async Task<bool> RenewCodeVerificatioToSms(string signature)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(user.Id);
            if (entity != null)
            {
                if (await ExistPhoneNumber(entity.Celular, entity.CodigoPais, user.Id))
                {
                    throw new ValidationException(new List<System.ComponentModel.DataAnnotations.ValidationResult> { new System.ComponentModel.DataAnnotations.ValidationResult("Número de celular ya utilizado", new List<string> { "Celular" }) });
                }
                entity.Signature = signature;
                entity.CambiarTelefono(entity.Celular, entity.CodigoPais);
                _ = await repository.UpdateCelular(entity);
                await eventBus.Publish(entity.PullDomainEvents());
                return true;
            }
            return false;
        }

        public async Task<bool> ExistEmail(string email)
        {
            return (await repository.GetByUserEmail(email)) != null;
        }

        public async Task<bool> ExistPhoneNumber(string phoneNumber, string countryCode, string afiliadoId = "")
        {
            countryCode = countryCode.StartsWith("+") ? countryCode : $"+{countryCode}";
            phoneNumber = phoneNumber.StartsWith("+") ? phoneNumber : $"+{phoneNumber}";

            if (validatePhone.Validate(phoneNumber, countryCode))
            {
                return ((await repository.GetByUserPhoneNumber(phoneNumber))?.Id ?? afiliadoId) != afiliadoId;
            }
            throw new NumeroTelefonoException();
        }

        public async Task<bool> ExistNumeroIdentidad(string numeroIdentidad)
        {
            return (await repository.GetByUserNumerodeIdentidad(numeroIdentidad)) != null;
        }

        public Task<List<Location>> GetLocations(double lat, double lon)
        {
            return location.GetDirectionByPos(lat, lon);
        }

        public async Task<bool> CambiarClave(UsuarioAfiliado model)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var entity = await repository.GetById(user.Id);
            if (entity.ValidatePassword(model.Password))
            {
                entity.CambiarPassword(model.Salt);
                entity.Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = user.Id };
                var result = await repository.UpdatePassword(entity);
                await eventBus.Publish(model.PullDomainEvents());
                return result;
            }
            return false;
        }

        public Task<bool> SetCominucadoBorrado(string id)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            return repository.ComunicadoDelete(user.Id, id);
        }

        public async Task<bool> SetCominucadoLeido(string id)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var com = await repository.GetComunicacionesByUser(user.Id, id);
            var result = await repository.ComunicadoLeido(user.Id, id);
            if (result)
            {
                await comunicacionRepository.UpdateEstadoComunicacion(com.IdComunicado, user.Id, com.Id!, Estados.Leido);
            }

            return result;
        }

        public Task<List<ComunicacionAfiliado>> GetComunicacionesByUser()
        {
            var user = authUser.GetAuthUser<AuthUser>();
            return repository.GetComunicacionesByUser(user.Id);
        }

        public Task<int> GetComunicacionesByUserCount()
        {
            var user = authUser.GetAuthUser<AuthUser>();
            return repository.GetComunicacionesByUserCount(user.Id);
        }

        public async Task<bool> RenewNotificationToken(UsuarioAfiliado usuarioAfiliado)
        {
            var user = authUser.GetAuthUser<AuthUser>();
            var afiliado = await repository.GetById(user.Id);

            if (afiliado.Token != usuarioAfiliado.Token)
            {
                if (!string.IsNullOrWhiteSpace(afiliado.Token))
                    await notificationRepository.RemoveTopics(afiliado.Token, afiliado.Id);

                afiliado.Token = usuarioAfiliado.Token;
                await repository.UpdateToken(afiliado);
            }
            return true;
        }
    }
}
