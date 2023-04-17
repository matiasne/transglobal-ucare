using GQ.Data.Dto;

namespace UCare.Web.Controllers.Config.Dto
{
    public class ConfigDto : Dto<Domain.Config.Config, ConfigDto>
    {
        public string? Id { get; set; }
        public long? UsuarioActivosMaximos { get; set; } = 5000;
        public long? TiempoEnvioSMSSeconds { get; set; } = 60;
        public long? MonitorPausaTimeOut { get; set; } = 15;
        public long? ConfirmarTimeOut { get; set; } = 60;
        public long? TiempoParaReasignarAlerta { get; set; } = 600;
    }
}