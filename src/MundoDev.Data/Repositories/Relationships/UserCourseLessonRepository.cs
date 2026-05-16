using MundoDev.Business.Interfaces.Internals.Relationships;
using MundoDev.Business.Models.Domains.Relationships;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Relationships
{
    public class UserCourseLessonRepository : Repository<UserCourseLesson>, IUserCourseLessonRepository
    {
        public UserCourseLessonRepository(MainDbContext db) : base(db) { }
    }
}
