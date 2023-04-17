using GQ.Architecture.DDD.Domain.Aggregate;
using GQ.Architecture.DDD.Domain.Repository;
using UCare.Shared.Infrastructure;

namespace UCare.Shared.Domain
{
    public interface IRepositoryFirebase<TEntity, TPrimary> : IRepository where TEntity : AggregateRoot<TPrimary>
    {
        Task<TEntity> Insert(TEntity entity);
        Task<bool> Update(TEntity model);
        Task Delete(TPrimary id);
        Task<TEntity> GetById(TPrimary id);
        Task<IPaging> Get(IPaging paging);

    }
}
