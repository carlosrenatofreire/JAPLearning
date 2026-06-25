using MundoDev.Business.Interfaces.Internals.Relationships;
using MundoDev.Business.Models.Domains.Relationships;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Relationships
{
    public class UserLessonTestRepository : Repository<UserLessonTest>, IUserLessonTestRepository
    {
        public UserLessonTestRepository(MainDbContext db) : base(db) { }
    }
}
