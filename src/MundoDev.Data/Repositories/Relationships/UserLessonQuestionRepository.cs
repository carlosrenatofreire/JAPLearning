using MundoDev.Business.Interfaces.Internals.Relationships;
using MundoDev.Business.Models.Domains.Relationships;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Relationships
{
    public class UserLessonQuestionRepository : Repository<UserLessonQuestion>, IUserLessonQuestionRepository
    {
        public UserLessonQuestionRepository(MainDbContext db) : base(db) { }
    }
}
