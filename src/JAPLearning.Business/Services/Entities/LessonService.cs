using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Validations.Internals.Entities;

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
            if (!await ValidateAsync(new LessonValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Lesson entity)
        {
            if (!await ValidateAsync(new LessonValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            if (await _repository.HasProgressRecordsAsync(id))
            {
                _notificator.AddNotification("Não é possível eliminar esta lição porque está associada a registos de progresso de formandos.");
                return false;
            }
            return await base.DeleteAsync(id);
        }
    }
}
