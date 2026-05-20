using JAPLearning.Business.Interfaces.Internals.Relationships;
using JAPLearning.Business.Models.Domains.Relationships;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Relationships
{
    public class UserLessonQuestionRepository : Repository<UserLessonQuestion>, IUserLessonQuestionRepository
    {
        public UserLessonQuestionRepository(MainDbContext db) : base(db) { }
    }
}
