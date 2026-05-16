using MundoDev.Business.Interfaces.Internals.Relationships;
using MundoDev.Business.Models.Domains.Relationships;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Relationships
{
    public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(MainDbContext db) : base(db) { }
    }
}
