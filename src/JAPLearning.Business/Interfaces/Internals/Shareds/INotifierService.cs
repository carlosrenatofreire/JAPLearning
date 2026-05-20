using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Interfaces.Internals.Shareds
{
    public interface INotifierService
    {
        bool HasNotification();
        List<Notification> GetNotification();
        void Handle(Notification notification);
    }
}
