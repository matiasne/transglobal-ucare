using UCare.Shared.Domain;

namespace UCare.Domain.Users
{
    public interface IUsuarioManagerRepository : IRepositoryFirebase<UsuarioManager, string?>
    {
        Task<List<UsuarioManager>> GetByRol(string rol);
        Task<UsuarioManager?> GetByUserName(string userName);
        Task<UsuarioManager?> GetByUserEmail(string email);
        Task<List<UsuarioManager>> GetUsersByUserId(string id);
        Task<List<UsuarioManager>> GetUsersByIdRol(string id);
        Task<bool> UpdateUsuarioId(UsuarioManager entity);
        Task<bool> UpdateMapaConfig(string id, MapaConfig entity);
        Task<bool> InsertAu(UsuarioAuth au);
        Task<List<UsuarioAuth>> GetAuthByUser(string id);
        Task<bool> RemoveAuthByUser(string id, string authId);
        Task<bool> RegistrarHistorial(string id, UsuarioManagerHistorial entity);
        Task<List<UsuarioManager>> GetAllUserManager();
        Task<bool> UpdatePassword(UsuarioManager user);
    }
}

