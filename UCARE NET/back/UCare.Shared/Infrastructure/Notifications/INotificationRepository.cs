using GQ.Architecture.DDD.Domain.Repository;

namespace UCare.Shared.Infrastructure.Notifications
{
    public interface INotificationRepository : IRepository
    {
        Task<bool> RemoveTopics(string token, string id);
    }
}
