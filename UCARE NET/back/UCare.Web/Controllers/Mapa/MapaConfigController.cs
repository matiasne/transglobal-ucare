using GQ.Security.Internal;
using Microsoft.AspNetCore.Mvc;
using UCare.Application.Mapa;
using UCare.Shared.Domain.ValueObjects;
using UCare.Web.Controllers.Mapa.Dto;

namespace UCare.Web.Controllers.Mapa
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [SecurityDescription("Mapa Config", new object[] { Roles.Administrador })]
    public class MapaConfigController : Controller
    {
        private readonly MapaApp app;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public MapaConfigController(MapaApp app)
        {
            this.app = app;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]")]
        public virtual async Task<MapaConfigDto> GetId()
        {
            var entity = (await app.GetMapaConfig());
            return new MapaConfigDto().SetEntity(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpPut($"api/v1/[controller]")]
        public virtual async Task<bool> Save([FromBody] MapaConfigDto model)
        {
            return await app.SaveConfigMapa(model.GetEntity());
        }
    }
}
