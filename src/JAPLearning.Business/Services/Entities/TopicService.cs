using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Validations.Internals.Entities;

namespace JAPLearning.Business.Services.Entities
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
            if (!await ValidateAsync(new TopicValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Topic entity)
        {
            if (!await ValidateAsync(new TopicValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }

        public async Task<List<Topic>> GetByCourseAsync(Guid courseId) =>
            (await _repository.Find(t => t.CourseId == courseId)).OrderBy(t => t.Order).ToList();
    }
}
