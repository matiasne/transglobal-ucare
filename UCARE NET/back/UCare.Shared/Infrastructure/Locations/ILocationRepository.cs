using GQ.Architecture.DDD.Domain.Repository;
using UCare.Shared.Domain.Locations;

namespace UCare.Shared.Infrastructure.Locations
{
    public interface ILocationRepository : IRepository
    {
        Task<List<Location>> GetDirectionByPos(double lat, double lon);
    }
}
