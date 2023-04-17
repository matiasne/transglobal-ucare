using GQ.Architecture.DDD.Domain;
using GQ.Architecture.DDD.Domain.Auth;
using GQ.Architecture.DDD.Domain.Bus.Event;
using GQ.Core.utils;
using UCare.Application.Authentication.Exceptions;
using UCare.Domain.Users;
using UCare.Shared.Application;
using UCare.Shared.Domain.Auth;
using UCare.Shared.Domain.ValueObjects;

namespace UCare.Application.Authentication
{
    public class AuthenticacionApp : ApplicationBase
    {
        private readonly IUsuarioManagerRepository managerRepository;
        private readonly IUsuarioAfiliadoRepository afiliadoRepository;
        private readonly IUniqueSession uniqueSession;

        public AuthenticacionApp(IUniqueSession uniqueSession, IUsuarioManagerRepository managerRepository, IUsuarioAfiliadoRepository afiliadoRepository, IAuthUserRepository authUser, IEventBus eventBus, ICacheService cacheService) : base(authUser, eventBus, cacheService)
        {
            this.managerRepository = managerRepository;
            this.afiliadoRepository = afiliadoRepository;
            this.uniqueSession = uniqueSession;
        }

        //Login para managers
        public async Task<string> LoginManagers(string emailOrUsername, string clave, string plataforma)
        {
            if (string.IsNullOrWhiteSpace(emailOrUsername) || string.IsNullOrWhiteSpace(clave))
                throw new UsuarioValidarEmailException();
            var user = await managerRepository.GetByUserEmail(emailOrUsername.ToLower());
            if (user?.Rol == Roles.Afiliado)
                user = null;
            return await ValidarUserLogin(user, clave);
        }

        //Login para afiliados
        public async Task<string> LoginAfiliados(string emailOrUsername, string clave, string plataforma)
        {
            if (string.IsNullOrWhiteSpace(emailOrUsername) || string.IsNullOrWhiteSpace(clave))
                throw new UsuarioValidarEmailException();

            Usuario? user = await afiliadoRepository.GetByUserEmail(emailOrUsername.ToLower());
            user = user ?? await afiliadoRepository.GetByUserNumerodeIdentidad(emailOrUsername);

            if (user?.Rol != Roles.Afiliado)
                user = null;

            return await ValidarUserLogin(user, clave);

        }

        private Task<string> ValidarUserLogin(Usuario? user, string clave)
        {
      
            if (user != null)
            {
                if (user.Rol == Roles.Afiliado || user.Estado == Estados.Activo)
                {
#if !DEBUG
                    if (user.ValidatePassword(clave))
#else
                    if (true)
#endif
                    {
                        var auth = new AuthUser { Id = user.Id ?? "", Name = user.UsuarioNombre ?? "", Rol = user.Rol, Estado = user.Estado, CodeSession = FunctionsUtils.CreateRandomCode(16), TenantId = user.TenantId };
                        uniqueSession.AddUser(auth);
                        return Task.FromResult(authUser.SingIn(auth));
                    }
                    else
                        throw new UsuarioIncorrectoException();
                }
                else
                    throw new UsuarioInactivoException();
            }
            throw new UsuarioNoExisteException();
        }

        public string RenewToken()
        {
            var auth = authUser.GetAuthUser<AuthUser>();
            if (auth == null)
                throw new UsuarioNoExisteException();
            return authUser.SingIn(auth);
        }

        public AuthUser GetUserLogin()
        {
            return authUser.GetAuthUser<AuthUser>();
        }

        public async Task<string> RecuperarContrasena(string emailOrUsername, string signature)
        {
            Usuario? user = await managerRepository.GetByUserEmail(emailOrUsername);
            //user = user ?? await afiliadoRepository.GetByUserPhoneNumber(emailOrUsername);

            //Si es afiliado solo debe loguearse con numero de identidad
            if (user == null || user!.Rol == Roles.Afiliado)
                user = await afiliadoRepository.GetByUserNumerodeIdentidad(emailOrUsername);

            if (user != null)
            {
                var aud = await managerRepository.GetAuthByUser(user.Id);

                if (aud.Count(x => x.Expiration > DateTime.UtcNow) > 2)
                {
                    throw new Exception("maximo_pedido_recuperacion");
                }

                var au = user.RecuperarPassword(signature);

                if (await managerRepository.InsertAu(au))
                {
                    await eventBus.Publish(user.PullDomainEvents());
                    return au.IdKey ?? "";
                }
            }
            throw new UsuarioNoExisteException();
        }

        public bool Logout(string plataforma)
        {
            uniqueSession.RemoveUser(authUser.GetAuthUser<AuthUser>());
            authUser.SingOut();
            return true;
        }

        public async Task<bool> VerificarCodigo(string id, string codigo)
        {
            var ids = id.Split("|");
            var aud = await managerRepository.GetAuthByUser(ids[0]);
            var au = aud.FirstOrDefault(x => x.IdKey == id);
            if (au != null && au.Code == codigo && DateTime.UtcNow < au.Expiration.ToUniversalTime())
                return true;
            return false;
        }

        public async Task<bool> CambiarPassword(string id, string codigo, string password)
        {
            if (await VerificarCodigo(id, codigo))
            {
                var ids = id.Split("|");
                var user = await managerRepository.GetById(ids[0]);
                user.CambiarPassword(password);
                await managerRepository.UpdatePassword(user);
                await managerRepository.RemoveAuthByUser(ids[0], id);
                return true;
            }
            return false;
        }
    }
}
