using GQ.Data.Dto;
using UCare.Domain.Users;
using UCare.Web.Controllers.Authentication.Dto;

namespace UCare.Web.Controllers.Mapa.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class MapaConfigDto : Dto<MapaConfig, MapaConfigDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public string? ApiKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? Zoom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GeoPositionDto Center { get; set; } = new GeoPositionDto();
    }
}
