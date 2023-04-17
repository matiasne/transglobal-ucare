using UCare.Shared.Domain;

namespace UCare.Domain.Config
{
    public  class Config : EntityBase<string?>
    {
        public long? UsuarioActivosMaximos { get; set; } = 5000;
        public long? TiempoEnvioSMSSeconds { get; set; } = 60;
        public long? MonitorPausaTimeOut { get; set; } = 15;
        public long? ConfirmarTimeOut { get; set; } = 60;
        public long? TiempoParaReasignarAlerta { get; set; } = 600;

    }
}
