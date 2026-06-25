using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Interfaces.Internals.Entities
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<bool> HasProgressRecordsAsync(Guid lessonId);
    }
}
