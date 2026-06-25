using MundoDev.Business.Notifications;

namespace MundoDev.Business.Interfaces.Internals.Shareds
{
    public interface INotificator
    {
        bool HasNotifications { get; }
        IReadOnlyList<Notification> GetNotifications();
        void AddNotification(string message);
        void AddNotification(Notification notification);
    }
}
