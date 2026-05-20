using JAPLearning.Business.Interfaces.Internals.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Parameters
{
    public class LevelRepository : Repository<Level>, ILevelRepository
    {
        public LevelRepository(MainDbContext db) : base(db) { }
    }
}
