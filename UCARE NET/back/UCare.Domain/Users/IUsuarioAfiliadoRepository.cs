using UCare.Shared.Domain;
using UCare.Shared.Domain.ValueObjects;

namespace UCare.Domain.Users
{
    public interface IUsuarioAfiliadoRepository : IRepositoryFirebase<UsuarioAfiliado, string?>
    {
        //Task<List<string>> GetBySexoAndEdadAndCodigoPostal(string sexo, int edadMinima, int edadMaxima, List<string>? codigoPostal);
        Task<bool> UpdateCelular(UsuarioAfiliado model);
        Task<bool> UpdateContactos(UsuarioAfiliado model);
        Task<bool> UpdateDatosPersonales(UsuarioAfiliado model);
        Task<bool> UpdatePatologias(UsuarioAfiliado model);
        Task<UsuarioAfiliado?> GetByUserName(string userName);
        Task<UsuarioAfiliado?> GetByUserEmail(string userEmail);
        Task<UsuarioAfiliado?> GetByUserPhoneNumber(string phoneNumber);
        Task<bool> UpdateEstado(UsuarioAfiliado model);
        Task<bool> UpdateNumeroIdentidad(UsuarioAfiliado UpdateCelular);
        Task<bool> UpdatePassword(UsuarioAfiliado model);
        Task<List<ComunicacionAfiliado>> GetComunicacionesByUser(string usuarioId);
        Task<ComunicacionAfiliado> GetComunicacionesByUser(string usuarioId, string id);
        Task<int> GetComunicacionesByUserCount(string usuarioId);
        Task<bool> ComunicadoLeido(string usuarioId, string comunicacionId);
        Task<bool> ComunicadoDelete(string usuarioId, string comunicacionId);
        Task<string> ComunicadoAdd(string id, ComunicacionAfiliado comunicado);
        Task<UsuarioAfiliado?> GetByUserNumerodeIdentidad(string numeroIdentidad);
        Task<long> GetCountActivos();
        Task<bool> UpdateToken(UsuarioAfiliado afiliado);

    }
}
