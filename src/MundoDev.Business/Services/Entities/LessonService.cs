using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Services.Entities
{
    public class LessonService : BaseService<Lesson, ILessonRepository>, ILessonService
    {
        public LessonService(IUnitOfWork uow, ILessonRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }
    }
}
