using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Services.Entities
{
    public class TopicService : BaseService<Topic, ITopicRepository>, ITopicService
    {
        public TopicService(IUnitOfWork uow, ITopicRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }

        public async Task<List<Topic>> GetByCourseAsync(Guid courseId) =>
            (await _repository.Find(t => t.CourseId == courseId)).OrderBy(t => t.Order).ToList();
    }
}
