using GQ.Architecture.DDD.Domain.Exceptions;
using GQ.Security.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using UCare.Application.Alertas;
using UCare.Application.Authentication;
using UCare.Application.Users;
using UCare.Shared.Domain.Auth;
using UCare.Shared.Domain.Locations;
using UCare.Web.Controllers.AlertasEstado.Dto;
using UCare.Web.Controllers.Authentication.Dto;

namespace UCare.Web.Controllers.Authentication
{
    /// <summary>
    /// Controler para login de usuarios
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly AuthenticacionApp autenticacion;
        private readonly UsuarioAfiliadoApp app;
        private readonly AlertaApp alertaApp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="autenticacion"></param>
        /// <param name="app"></param>
        /// <param name="alertaApp"></param>
        public LoginController(AuthenticacionApp autenticacion, UsuarioAfiliadoApp app, AlertaApp alertaApp)
        {
            this.autenticacion = autenticacion;
            this.app = app;
            this.alertaApp = alertaApp;
        }

        /// <summary>
        /// Metodo de acceso a la plataforma
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/v1/Login")]
        public async Task<string> LoginAfiliado([FromBody] LoginUsernamePasswordDto model)
        {
            return await autenticacion.LoginAfiliados(model.Username, model.Password, model.Plataforma);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/v1/[action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<string> LoginManager([FromBody] LoginUsernamePasswordDto model)
        {
            return await autenticacion.LoginManagers(model.Username, model.Password, model.Plataforma);
        }

        /// <summary>
        /// Se envia el email o el numero de telefono y se devulve un id para chequear con el codigo de verificacion que se envio por SMS o Email
        /// </summary>
        /// <param name="emailTelefono"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/v1/[action]")]
        public async Task<string> Recuperar([FromBody] RecupearDto emailTelefono)
        {
            return await autenticacion.RecuperarContrasena(emailTelefono.Username, emailTelefono.Signature);
        }

        /// <summary>
        /// Enviamos el Id que recibimos de la recuperacion ma sel codigo que recibimos por SMS o email
        /// </summary>
        /// <param name="model"></param
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/v1/[action]")]
        public async Task<bool> RecuperarVerificarCodigo([FromBody] VerificarCodigoRecuperarDto model)
        {
            return await autenticacion.VerificarCodigo(model.Id, model.Codigo);
        }

        /// <summary>
        /// Enviamos el Id que recibimos de la recuperacion mas el codigo que recibimos por SMS o email y el password nuevo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/v1/[action]")]
        public async Task<bool> RecuperarCambiar([FromBody] CambiarPasswordCodigoRecuperarDto model)
        {
            return await autenticacion.CambiarPassword(model.Id, model.Codigo, model.Password);
        }

        /// <summary>
        /// Se envia el email o el numero de telefono y se devulve un id para chequear con el codigo de verificacion que se envio por SMS o Email
        /// </summary>
        /// <param name="emailTelefono"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/v1/[action]")]
        public string Version()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        /// <summary>
        /// Con un token valido permite renovarlo
        /// </summary>
        /// <returns></returns>
        [SecurityDescription]
        [HttpGet("api/v1/[action]")]
        public string RenewToken()
        {
            return autenticacion.RenewToken();
        }

        /// <summary>
        /// Obtiene el usuario logueado en el sistema
        /// </summary>
        /// <returns></returns>
        [SecurityDescription]
        [HttpGet("api/v1/[action]")]
        public AuthUser GetUser()
        {
            return autenticacion.GetUserLogin();
        }

        /// <summary>
        /// Desloguea al usuario del sistema
        /// </summary>
        /// <param name="plataforma"></param>
        /// <returns></returns>
        [SecurityDescription]
        [HttpGet("api/v1/[action]/{plataforma?}")]
        public bool Logout(string plataforma)
        {
            return autenticacion.Logout(plataforma);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Se registra el Afiliado</response>
        /// <response code="400">Errores de validacion</response>
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(ValidationException), Description = "Error de parametros.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost($"api/v1/[action]")]
        public async Task<UsuarioAfiliadoDto> Registrar([FromBody] UsuarioAfiliadoDto model)
        {
            return model.SetEntity(await app.Registrar(model.GetEntity()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet($"api/v1/[action]/{{email}}")]
        public async Task<bool> ExistEmail(string email)
        {
            return await app.ExistEmail(email);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryCode">Codigo pasis sin el signo mas +</param>
        /// <param name="phoneNumber">Numero de telefono completo con codigo pasis sin el signo +</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet($"api/v1/[action]/{{countryCode}}/{{phoneNumber}}")]
        public async Task<bool> ExistPhoneNumber(string countryCode, string phoneNumber)
        {
            return await app.ExistPhoneNumber(phoneNumber, countryCode);
        }
        /// <summary>
        /// Enviamos una alerta al centro de monitoreo
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet($"api/v1/[controller]/[action]/{{lat}},{{lon}}")]
        public async Task<List<Location>> GetLocations(double lat, double lon)
        {
            return await app.GetLocations(lat, lon);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Position"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost($"api/v1/[controller]/[action]")]
        public async Task<bool> SendAlert([FromBody] GeoPositionDto Position)
        {
            return await alertaApp.SendAlert(Position.Id, Position.GetEntity());
        }

        /// <summary>
        /// Actualizamos la posicion de la alerta activa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Position"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost($"api/v1/[controller]/[action]/{{id}}")]
        public async Task<bool> UpdateAlert(string id, [FromBody] GeoPositionDto Position)
        {
            return await alertaApp.UpdateAlert(id, Position.GetEntity());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet($"api/v1/[controller]/[action]")]
        public async Task<AlertaDto?> GetAlert(string id)
        {
            var alerta = await alertaApp.GetAlertByUser(id);
            if (alerta != null)
                return new AlertaDto().SetEntity(alerta);
            return null;
        }
    }
}
