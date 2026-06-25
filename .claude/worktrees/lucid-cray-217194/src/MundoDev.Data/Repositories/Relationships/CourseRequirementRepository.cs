using MundoDev.Business.Interfaces.Internals.Relationships;
using MundoDev.Business.Models.Domains.Relationships;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Relationships
{
    public class CourseRequirementRepository : Repository<CourseRequirement>, ICourseRequirementRepository
    {
        public CourseRequirementRepository(MainDbContext db) : base(db) { }
    }
}
