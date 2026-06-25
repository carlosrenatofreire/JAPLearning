using MundoDev.Business.Interfaces.Internals.Auxiliaries;
using MundoDev.Business.Models.Domains.Auxiliaries;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Auxiliaries
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(MainDbContext db) : base(db) { }
    }
}
