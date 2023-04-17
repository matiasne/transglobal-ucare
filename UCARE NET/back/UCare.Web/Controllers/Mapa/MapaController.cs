using GQ.Core.service;
using GQ.Data;
using GQ.Security.Internal;
using Microsoft.AspNetCore.Mvc;
using UCare.Application.Mapa;
using UCare.Domain.Alertas;
using UCare.Infrastructure.Firebase;
using UCare.Shared.Domain.ValueObjects;
using UCare.Shared.Infrastructure;
using UCare.Web.Controllers.AlertasEstado.Dto;
using UCare.Web.Controllers.Mapa.Dto;

namespace UCare.Web.Controllers.Mapa
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [SecurityDescription("Mapa", new object[] { Roles.Administrador })]
    public class MapaController : Controller
    {
        private readonly MapaApp app;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public MapaController(MapaApp app)
        {
            this.app = app;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpPost($"api/v1/[controller]")]
        public async Task<IPaging> Find([FromBody] Paging<Alerta, AlertaDto> paging)
        {
            return await app.Get(paging);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]")]
        public async Task<MapaConfigDto> GetMapaConfig()
        {
            var dto = new MapaConfigDto().SetEntity(await app.GetMapaConfig());
            dto.ApiKey = ServicesContainer.Configuration["Mapa:Google:Key"];
            return dto;
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
