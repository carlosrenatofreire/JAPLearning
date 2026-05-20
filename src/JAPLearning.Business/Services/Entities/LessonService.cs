using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Validators;

namespace JAPLearning.Business.Services.Entities
{
    public class LessonService : AuditableService<Lesson, ILessonRepository>, ILessonService
    {
        public LessonService(
            IUnitOfWork uow,
            ILessonRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog) { }

        public override async Task<bool> AddAsync(Lesson entity)
        {
            if (!Validate(new LessonValidator(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Lesson entity)
        {
            if (!Validate(new LessonValidator(), entity)) return false;
            return await base.UpdateAsync(entity);
        }
    }
}
