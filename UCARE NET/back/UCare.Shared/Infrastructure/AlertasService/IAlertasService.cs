namespace UCare.Shared.Infrastructure.AlertasService
{
    public interface IAlertasService
    {
        Task<bool> AddMonitor(string id, string connectionId);
        Task<bool> ConfirmarAsignacion(string monitorId, string alertaId);
        Task<bool> AddAlerta(string id);
        Task<bool> IsJoin(string id);
        Task<bool> RemoveMonitor(string id);
        void DisconnectedMonitor(string id, string connectionId);
        Task<bool> CambiarEstado(string id, string alertaId, string estado);
        Task<bool> FinalizarAsistencia(string id, string alertaId, string bitacora);
        Task<bool> ActivarDesactivarAlarma(string id, string alertaId);
        Task<bool> Descanso(string id);
        Task<bool> Iniciar(string id);
        void ChangeConfig();
        Task<List<dynamic>> GetAlertaByUser(string id);
        void ConnectedMonitor(string? id, string connectionId);
        Task UpdatePosition(string alertaId, double lat, double lon);
    }
}
