using UCare.Shared.Domain;

namespace UCare.Domain.Alertas
{
    public interface IAlertaRepository : IRepositoryFirebase<Alerta, string?>
    {
        Task<Alerta?> GetAlertByUserId(string id);
        Task<List<Alerta>> GetPendientes();
        Task<List<Alerta>> GetPendientesAsignados();
        Task<List<Alerta>> GetPendientesAsignados(string id);
        Task<bool> UpdateAlarma(Alerta model);
        Task<bool> UpdateAsignare(Alerta model);
        Task<bool> UpdateBitacora(Alerta entity);
        Task<bool> UpdateConfirmarAsignacion(Alerta model);
        Task<bool> UpdateFinalizacion(Alerta model);
        Task<bool> UpdateState(Alerta model);
        Task<bool> UpdateUbicacione(Alerta entity);
        Task<bool> UpdatePosition(Alerta entity);
    }
}

