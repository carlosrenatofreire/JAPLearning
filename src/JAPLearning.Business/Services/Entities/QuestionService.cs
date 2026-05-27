using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Services.Entities
{
    public class QuestionService : BaseService<Question, IQuestionRepository>, IQuestionService
    {
        public QuestionService(IUnitOfWork uow, IQuestionRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }

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
