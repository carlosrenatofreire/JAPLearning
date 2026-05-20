using JAPLearning.Business.Interfaces.Services;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Interfaces.Services.Entities
{
    public interface IQuestionService : IBaseService<Question>
    {
        Task<List<Question>> GetByLessonAsync(Guid lessonId);
    }
}
