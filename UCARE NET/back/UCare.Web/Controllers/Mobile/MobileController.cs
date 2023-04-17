using GQ.Security.Internal;
using Microsoft.AspNetCore.Mvc;
using UCare.Application.Alertas;
using UCare.Application.Users;
using UCare.Shared.Domain.ValueObjects;
using UCare.Web.Controllers.AlertasEstado.Dto;
using UCare.Web.Controllers.Authentication.Dto;
using UCare.Web.Controllers.Mobile.Dto;

namespace UCare.Web.Controllers.Mobile
{
    /// <summary>
    /// Controlador para la app mobile
    /// </summary>
    [ApiController]
    [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
    public class MobileController : Controller
    {
        private readonly UsuarioAfiliadoApp afiliadoApp;
        private readonly AlertaApp alertaApp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="afiliadoApp"></param>
        /// <param name="alertaApp"></param>
        public MobileController(UsuarioAfiliadoApp afiliadoApp, AlertaApp alertaApp)
        {
            this.afiliadoApp = afiliadoApp;
            this.alertaApp = alertaApp;
        }

        /// <summary>
        /// Renueva el token de FireBase para las notificaciones y desuscribe al token anterior de
        /// las notificaciones
        /// </summary>
        /// <param name="token">TOKEN de Firebase</param>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpPost($"api/v1/[controller]/[action]")]
        public async Task<bool> RenewNotificationToken([FromBody] TokenDto token)
        {
            return await afiliadoApp.RenewNotificationToken(token.GetEntity());
        }
        
        /// <summary>
        /// Reenvia el mensaje de SMS con el codigo de verificacion
        /// </summary>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpPost($"api/v1/[controller]/[action]")]
        public async Task<bool> RenewCodeVerificatioToSms([FromBody] TokenDto token)
        {
            return await afiliadoApp.RenewCodeVerificatioToSms(token.Token);
        }

        /// <summary>
        /// Comprobamos si el codigo de verificacion es correcto
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpGet($"api/v1/[controller]/[action]/{{codigo}}")]
        public async Task<bool> VerifyCodeBySms(string codigo)
        {
            return await afiliadoApp.VerifyCodeBySms(codigo);
        }

        /// <summary>
        /// Recuperamos los datos personales del afiliado
        /// </summary>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpGet($"api/v1/[controller]/[action]")]
        public async Task<DatosPersonalesDto> GetDatosPersonales()
        {
            return new DatosPersonalesDto().SetEntity(await afiliadoApp.GetAfiliado());
        }

        /// <summary>
        /// Guardamos los datos Personales del afiliado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpPost($"api/v1/[controller]/[action]")]
        public async Task<DatosPersonalesDto> SetDatosPersonales([FromBody] DatosPersonalesDto model)
        {
            return new DatosPersonalesDto().SetEntity(await afiliadoApp.CambiarDatosPersonales(model.GetEntity()));
        }

        /// <summary>
        /// Recuperamos el numero de identidad del afiliado
        /// </summary>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpGet($"api/v1/[controller]/[action]")]
        public async Task<NumeroIdentidadDto> GetNumeroIdentidad()
        {
            return new NumeroIdentidadDto().SetEntity(await afiliadoApp.GetAfiliado());
        }

        /// <summary>
        /// Guardamos el numero  de identidad del afiliado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpPost($"api/v1/[controller]/[action]")]
        public async Task<NumeroIdentidadDto> SetNumeroIdentidad([FromBody] NumeroIdentidadDto model)
        {
            return new NumeroIdentidadDto().SetEntity(await afiliadoApp.CambiarNumeroIdentidad(model.GetEntity()));
        }

        /// <summary>
        /// Recuperamos los datos de contacto del afiliado
        /// </summary>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpGet($"api/v1/[controller]/[action]")]
        public async Task<ContactosEmergenciaDto> GetContactosEmergencia()
        {
            return new ContactosEmergenciaDto().SetEntity(await afiliadoApp.GetAfiliado());
        }

        /// <summary>
        /// Guardamos los contactos del afiliado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpPost($"api/v1/[controller]/[action]")]
        public async Task<ContactosEmergenciaDto> SetContactosEmergencia([FromBody] ContactosEmergenciaDto model)
        {
            return new ContactosEmergenciaDto().SetEntity(await afiliadoApp.CambiarContactos(model.GetEntity()));
        }

        /// <summary>
        /// Recuperamos las patologias y datos medicos del afiliado
        /// </summary>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpGet($"api/v1/[controller]/[action]")]
        public async Task<PatologiasDto> GetPatologias()
        {
            return new PatologiasDto().SetEntity(await afiliadoApp.GetAfiliado());
        }

        /// <summary>
        /// Guardamos las patologias y datos medicos del afiliado
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpPost($"api/v1/[controller]/[action]")]
        public async Task<PatologiasDto> SetPatologias([FromBody] PatologiasDto model)
        {
            return new PatologiasDto().SetEntity(await afiliadoApp.CambiarPatologias(model.GetEntity()));
        }

        /// <summary>
        /// Cambio de contraseña
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpPost($"api/v1/[controller]/[action]")]
        public async Task<bool> SetClave([FromBody] CambioContrasena model)
        {
            return await afiliadoApp.CambiarClave(model.GetEntity());
        }

        /// <summary>
        /// Enviamos una alerta al centro de monitoreo
        /// </summary>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpPost($"api/v1/[controller]/[action]")]
        public async Task<bool> SendAlert([FromBody] GeoPositionDto Position)
        {
            return await alertaApp.SendAlert(Position.GetEntity());
        }

        /// <summary>
        /// Actualizamos la posicion de la alerta activa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Position"></param>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpPost($"api/v1/[controller]/[action]/{{id}}")]
        public async Task<bool> UpdateAlert(string id, [FromBody] GeoPositionDto Position)
        {
            return await alertaApp.UpdateAlert(id, Position.GetEntity());
        }

        /// <summary>
        /// Obtenemos si el usuario tiene una alerta activa
        /// </summary>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpGet($"api/v1/[controller]/[action]")]
        public async Task<AlertaDto?> GetAlert()
        {
            var alerta = await alertaApp.GetAlertByUser();
            if (alerta != null)
                return new AlertaDto().SetEntity(alerta);
            return null;
        }

        /// <summary>
        /// Obtenemos 
        /// </summary>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpGet($"api/v1/[controller]/[action]")]
        public async Task<int> GetComunicadosSinLeer()
        {
            return await afiliadoApp.GetComunicacionesByUserCount();
        }

        /// <summary>
        /// Obtenemos la lista de comunicaciones
        /// </summary>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpGet($"api/v1/[controller]/[action]")]
        public async Task<List<ComunicadoDto>> GetComunicados()
        {
            return new ComunicadoDto().SetEntity(await afiliadoApp.GetComunicacionesByUser());
        }

        /// <summary>
        /// Se marca como leido la comunicacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpGet($"api/v1/[controller]/[action]/{{id}}")]
        public async Task<bool> ComunicadoLeido(string id)
        {
            return await afiliadoApp.SetCominucadoLeido(id);
        }

        /// <summary>
        /// Se borra una comunicacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SecurityDescription("Mobile", new object[] { Roles.Afiliado })]
        [HttpGet($"api/v1/[controller]/[action]/{{id}}")]
        public async Task<bool> ComunicadoBorrado(string id)
        {
            return await afiliadoApp.SetCominucadoBorrado(id);
        }
    }
}
