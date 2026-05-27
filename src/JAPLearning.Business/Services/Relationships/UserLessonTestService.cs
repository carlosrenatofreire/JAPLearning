using JAPLearning.Business.Interfaces.Internals.Relationships;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Relationships;
using JAPLearning.Business.Models.Domains.Relationships;

namespace JAPLearning.Business.Services.Relationships
{
    public class UserLessonTestService : BaseService<UserLessonTest, IUserLessonTestRepository>, IUserLessonTestService
    {
        public UserLessonTestService(IUnitOfWork uow, IUserLessonTestRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }

        public async Task<List<UserLessonTest>> GetByUserAsync(Guid userId)
        {
            var all = await _repository.GetAll();
            return all.Where(t => t.UserId == userId).ToList();
        }

        public async Task<UserLessonTest?> GetLatestByUserAndLessonAsync(Guid userId, Guid lessonId)
        {
            var all = await _repository.GetAll();
            return all
                .Where(t => t.UserId == userId && t.LessonId == lessonId)
                .OrderByDescending(t => t.CompletedDate)
                .FirstOrDefault();
        }
    }
}
