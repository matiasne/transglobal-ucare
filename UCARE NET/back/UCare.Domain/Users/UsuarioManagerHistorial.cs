namespace UCare.Domain.Users
{
    public static class UsuarioManagerHistorialAcciones
    {
        public const string CONECTARSE = "conectarse";
        public const string DESCONECTARSE = "desconectarse";
        public const string ENTRAR = "entrar";
        public const string SALIR = "salir";
        public const string PAUSAR = "pausar";
        public const string ASIGNAR = "asignar";
        public const string DESASIGNAR = "desasignar";
        public const string CONFIRMAR = "confirmar";
        public const string CAMBIAR_ESTADO = "cambairEstado";
        public const string ACTIVAR_ALARMA = "activarAlarma";
        public const string DESACTIVAR_ALARMA = "desactivarAlarma";
        public const string FINALIZAR = "finalizar";

        public static string[] ALL { get; private set; } = new string[] {
            CONECTARSE ,
            DESCONECTARSE ,
            ENTRAR ,
            SALIR,
            PAUSAR ,
            ASIGNAR,
            DESASIGNAR,
            CONFIRMAR,
            CAMBIAR_ESTADO,
            ACTIVAR_ALARMA,
            DESACTIVAR_ALARMA,
            FINALIZAR
        };
    }

    public class UsuarioManagerHistorial : GQ.Data.Abstractions.Entity.EntityBase<string?>
    {
        public virtual DateTime Fecha { get; set; } = DateTime.UtcNow;
        public virtual string Accion { get; set; } = string.Empty;
        public virtual string AlertId { get; set; } = string.Empty;
        public virtual string AfiliadoId { get; set; } = string.Empty;

        public static UsuarioManagerHistorial Create(string accion, string alertaId = "", string afiliadoId = "")
        {
            if (UsuarioManagerHistorialAcciones.ALL.Contains(accion))
                return new UsuarioManagerHistorial
                {
                    Accion = accion,
                    AlertId = alertaId,
                    AfiliadoId = afiliadoId
                };
            return null;
        }
    }
}
