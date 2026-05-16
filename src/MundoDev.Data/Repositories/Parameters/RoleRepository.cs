using MundoDev.Business.Interfaces.Internals.Parameters;
using MundoDev.Business.Models.Domains.Parameters;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Parameters
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(MainDbContext db) : base(db) { }
    }
}
