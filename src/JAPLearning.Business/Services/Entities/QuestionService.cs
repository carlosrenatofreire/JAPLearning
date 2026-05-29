using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;

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

        public async Task<List<Question>> GetByLessonAsync(Guid lessonId)
        {
            // GetAll already includes Options via ThenInclude in QuestionRepository
            var all = await _repository.GetAll();
            return all
                .Where(q => q.LessonId == lessonId && q.IsActived && !q.IsDeleted)
                .OrderBy(q => q.Name)
                .ToList();
        }
    }
}
