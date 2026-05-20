using JAPLearning.Business.Interfaces.Internals.Auxiliaries;
using JAPLearning.Business.Models.Domains.Auxiliaries;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Auxiliaries
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(MainDbContext db) : base(db) { }
    }
}
