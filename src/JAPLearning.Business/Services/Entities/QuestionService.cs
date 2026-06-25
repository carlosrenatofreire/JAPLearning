using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Validations.Internals.Entities;

namespace JAPLearning.Business.Services.Entities
{
    public class QuestionService : AuditableService<Question, IQuestionRepository>, IQuestionService
    {
        public QuestionService(
            IUnitOfWork uow,
            IQuestionRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog) { }

        public override async Task<bool> AddAsync(Question entity)
        {
            if (!await ValidateAsync(new QuestionValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Question entity)
        {
            if (!await ValidateAsync(new QuestionValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }

        public async Task<List<Question>> GetByLessonAsync(Guid lessonId)
        {
            var all = await _repository.GetAll();
            return all
                .Where(q => q.LessonId == lessonId && q.IsActived && !q.IsDeleted)
                .OrderBy(q => q.Name)
                .ToList();
        }
    }
}
