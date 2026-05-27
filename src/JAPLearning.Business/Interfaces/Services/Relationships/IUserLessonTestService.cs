using JAPLearning.Business.Interfaces.Services;
using JAPLearning.Business.Models.Domains.Relationships;

namespace JAPLearning.Business.Interfaces.Services.Relationships
{
    public interface IUserLessonTestService : IBaseService<UserLessonTest>
    {
        Task<List<UserLessonTest>> GetByUserAsync(Guid userId);
        Task<UserLessonTest?> GetLatestByUserAndLessonAsync(Guid userId, Guid lessonId);
    }
}
