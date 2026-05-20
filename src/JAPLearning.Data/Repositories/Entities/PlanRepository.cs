using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Entities
{
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {
        public PlanRepository(MainDbContext db) : base(db) { }
    }
}
