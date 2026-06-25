using MundoDev.Business.Interfaces.Internals.Relationships;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Relationships;
using MundoDev.Business.Models.Domains.Relationships;

namespace MundoDev.Business.Services.Relationships
{
    public class UserCourseLessonService : IUserCourseLessonService
    {
        private readonly IUserCourseLessonRepository _repository;
        private readonly IUnitOfWork _uow;

        public UserCourseLessonService(IUserCourseLessonRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public async Task<List<UserCourseLesson>> GetByUserAsync(Guid userId) =>
            await _repository.GetByUserAsync(userId);

        public async Task<UserCourseLesson?> GetByUserAndLessonAsync(Guid userId, Guid lessonId) =>
            await _repository.GetByUserAndLessonAsync(userId, lessonId);

        public async Task MarkCompletedAsync(Guid userId, Guid lessonId)
        {
            var existing = await _repository.GetByUserAndLessonAsync(userId, lessonId);
            if (existing != null)
            {
                existing.CompletedDate = DateTime.UtcNow;
                await _repository.Update(existing);
            }
            else
            {
                var record = new UserCourseLesson
                {
                    UserId        = userId,
                    LessonId      = lessonId,
                    CompletedDate = DateTime.UtcNow
                };
                await _repository.Add(record);
            }
            await _uow.Commit();
        }
    }
}
