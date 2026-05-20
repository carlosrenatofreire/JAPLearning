using JAPLearning.Business.Interfaces.Internals.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Parameters
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        public ModuleRepository(MainDbContext db) : base(db) { }
    }
}
