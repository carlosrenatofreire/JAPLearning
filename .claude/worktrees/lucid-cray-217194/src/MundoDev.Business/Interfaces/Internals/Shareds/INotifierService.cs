using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Interfaces.Internals.Shareds
{
    public interface INotifierService
    {
        bool HasNotification();
        List<Notification> GetNotification();
        void Handle(Notification notification);
    }
}
