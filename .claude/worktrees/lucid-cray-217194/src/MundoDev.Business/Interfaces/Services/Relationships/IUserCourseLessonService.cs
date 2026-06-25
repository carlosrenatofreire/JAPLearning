using MundoDev.Business.Models.Domains.Relationships;

namespace MundoDev.Business.Interfaces.Services.Relationships
{
    public interface IUserCourseLessonService
    {
        Task<List<UserCourseLesson>> GetByUserAsync(Guid userId);
        Task<UserCourseLesson?> GetByUserAndLessonAsync(Guid userId, Guid lessonId);
        Task MarkCompletedAsync(Guid userId, Guid lessonId);
    }
}
