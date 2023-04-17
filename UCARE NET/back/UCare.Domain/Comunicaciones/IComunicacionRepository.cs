using UCare.Shared.Domain;

namespace UCare.Domain.Comunicaciones
{
    public interface IComunicacionRepository : IRepositoryFirebase<Comunicacion, string?>
    {
        Task<List<Comunicacion>> GetPandientes();
        Task<bool> UpadeteEstado(Comunicacion comunicacion, List<ComunicacionEnvio> envios);
        Task<bool> UpdateEstadoComunicacion(string id, string idAfiliado, string idAfiliadoComunicado, string estado);
    }
}

