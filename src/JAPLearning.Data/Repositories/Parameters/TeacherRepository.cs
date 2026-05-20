using JAPLearning.Business.Interfaces.Internals.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Parameters
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(MainDbContext db) : base(db) { }
    }
}
