using MundoDev.Business.Interfaces.Services;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Interfaces.Services.Entities
{
    public interface IQuestionService : IBaseService<Question>
    {
        Task<List<Question>> GetByLessonAsync(Guid lessonId);
    }
}
