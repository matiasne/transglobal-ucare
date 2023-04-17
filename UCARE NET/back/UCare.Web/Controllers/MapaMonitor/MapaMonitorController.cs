using GQ.Core.service;
using GQ.Security.Internal;
using Microsoft.AspNetCore.Mvc;
using UCare.Application.Mapa;
using UCare.Shared.Domain.ValueObjects;
using UCare.Web.Controllers.AlertasEstado.Dto;
using UCare.Web.Controllers.Authentication.Dto;
using UCare.Web.Controllers.Mapa.Dto;

namespace UCare.Web.Controllers.MapaMonitor
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [SecurityDescription("Mapa", new object[] { Roles.Monitor })]
    public class MapaMonitorController : Controller
    {
        private readonly MapaApp app;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public MapaMonitorController(MapaApp app)
        {
            this.app = app;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]/[action]")]
        public async Task<MapaConfigDto> GetMapaConfig()
        {
            var dto = new MapaConfigDto().SetEntity(await app.GetMapaConfig());
            dto.ApiKey = ServicesContainer.Configuration["Mapa:Google:Key"];
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]/[action]")]
        public async Task<List<AlertaDto>> GetAsignados()
        {
            return new AlertaDto().SetEntity(await app.GetAsignados());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [SecurityDescription()]
        [HttpGet($"api/v1/[controller]/[action]/{{alertaId}}")]
        public async Task<UsuarioAfiliadoDto> GetMoreInfo(string alertaId)
        {
            return new UsuarioAfiliadoDto().SetEntity(await app.GetMoreInfo(alertaId));
        }


    }
}
