using Microsoft.AspNetCore.Http;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Auxiliaries;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Validators;

namespace MundoDev.Business.Services.Entities
{
    public class TopicService : AuditableService<Topic, ITopicRepository>, ITopicService
    {
        public TopicService(
            IUnitOfWork uow,
            ITopicRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog) { }

        public override async Task<bool> AddAsync(Topic entity)
        {
            if (!Validate(new TopicValidator(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Topic entity)
        {
            if (!Validate(new TopicValidator(), entity)) return false;
            return await base.UpdateAsync(entity);
        }

        public async Task<List<Topic>> GetByCourseAsync(Guid courseId) =>
            (await _repository.Find(t => t.CourseId == courseId)).OrderBy(t => t.Order).ToList();
    }
}
