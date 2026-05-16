using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Entities
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(MainDbContext db) : base(db) { }
    }
}
