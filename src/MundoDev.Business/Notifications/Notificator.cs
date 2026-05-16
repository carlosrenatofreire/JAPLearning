using MundoDev.Business.Interfaces.Internals.Shareds;

namespace MundoDev.Business.Notifications
{
    public class Notificator : INotificator
    {
        private readonly List<Notification> _notifications = new();

        public bool HasNotifications => _notifications.Any();

        public IReadOnlyList<Notification> GetNotifications() => _notifications.AsReadOnly();

        public void AddNotification(string message) =>
            _notifications.Add(new Notification(message));

        public void AddNotification(Notification notification) =>
            _notifications.Add(notification);
    }
}
