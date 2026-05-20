using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Models.Domains.Relationships;

namespace JAPLearning.Business.Interfaces.Internals.Relationships
{
    public interface IUserCourseLessonRepository : IRepository<UserCourseLesson>
    {
        Task<List<UserCourseLesson>> GetByUserAsync(Guid userId);
        Task<UserCourseLesson?> GetByUserAndLessonAsync(Guid userId, Guid lessonId);
    }
}
