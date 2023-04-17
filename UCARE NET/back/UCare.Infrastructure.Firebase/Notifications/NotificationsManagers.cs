using FirebaseAdmin.Messaging;
using UCare.Shared.Infrastructure.Notifications;

namespace UCare.Infrastructure.Firebase.Notifications
{
    public class NotificationsManagers : INotificationRepository
    {
        public async Task<bool> RemoveTopics(string token, string id)
        {
            var tokens = new List<string> { token };

            _ = await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(tokens, id);
            _ = await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(tokens, $"{id}-alarma");

            return true;
        }
    }
}
