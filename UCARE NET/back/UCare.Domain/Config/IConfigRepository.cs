using UCare.Shared.Domain;

namespace UCare.Domain.Config
{
    public interface IConfigRepository : IRepositoryFirebase<Config, string?>
    {
    }
}

