using JAPLearning.Business.Interfaces.Internals.Relationships;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Relationships;
using JAPLearning.Business.Models.Domains.Relationships;

namespace JAPLearning.Business.Services.Relationships
{
    public class UserCourseLessonService : IUserCourseLessonService
    {
        private readonly IUserCourseLessonRepository _repository;
        private readonly IUnitOfWork                 _uow;
        private readonly IAuditLogService            _auditLog;

        public UserCourseLessonService(
            IUserCourseLessonRepository repository,
            IUnitOfWork uow,
            IAuditLogService auditLog)
        {
            _repository = repository;
            _uow        = uow;
            _auditLog   = auditLog;
        }

        public async Task<List<UserCourseLesson>> GetAllAsync() =>
            await _repository.GetAll();

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

            // Regista conclusão de aula na auditoria
            try
            {
                await _auditLog.LogInfoAsync(
                    createdBy:  userId.ToString(),
                    action:     "LessonCompleted",
                    entityName: "UserCourseLesson",
                    json:       $"{{\"UserId\":\"{userId}\",\"LessonId\":\"{lessonId}\"}}");
            }
            catch { /* falha no log não interrompe o fluxo */ }
        }
    }
}
