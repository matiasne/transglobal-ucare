using GQ.Security.Internal;
using Microsoft.AspNetCore.Mvc;
using UCare.Application.Users;
using UCare.Shared.Domain.ValueObjects;
using UCare.Web.Controllers.Config.Dto;

namespace UCare.Web.Controllers.Config
{
    [ApiController]
    [SecurityDescription("Configuraciones", new object[] { Roles.Propietario, Roles.Gerente })]
    public class ConfigController : Controller
    {
        private readonly ConfigCrudApp app;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ap"></param>
        public ConfigController(ConfigCrudApp app)
        {
            this.app = app;
        }

        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]")]
        public virtual async Task<ConfigDto> GetId()
        {
            var entity = (await app.GetById("")) ?? new Domain.Config.Config();
            return new ConfigDto().SetEntity(entity);
        }

        [SecurityDescription("Guardar", new object[] { Roles.Propietario })]
        [HttpPut($"api/v1/[controller]/[action]")]
        public virtual async Task<ConfigDto> SaveUsuarioActivosMaximos([FromBody] ConfigDto model)
        {
            model.TiempoEnvioSMSSeconds = null;
            model.ConfirmarTimeOut = null;
            model.MonitorPausaTimeOut = null;
            model.TiempoParaReasignarAlerta = null;
            return model.SetEntity(await app.Save(model.GetEntity()));
        }
        [SecurityDescription("Guardar", new object[] { Roles.Gerente })]
        [HttpPut($"api/v1/[controller]/[action]")]
        public virtual async Task<ConfigDto> SaveTiempoEnvioSMSSeconds([FromBody] ConfigDto model)
        {
            model.UsuarioActivosMaximos = null;
            return model.SetEntity(await app.Save(model.GetEntity()));
        }
    }
}
