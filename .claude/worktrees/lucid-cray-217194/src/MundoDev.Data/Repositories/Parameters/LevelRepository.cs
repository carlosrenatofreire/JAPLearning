using MundoDev.Business.Interfaces.Internals.Parameters;
using MundoDev.Business.Models.Domains.Parameters;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Parameters
{
    public class LevelRepository : Repository<Level>, ILevelRepository
    {
        public LevelRepository(MainDbContext db) : base(db) { }
    }
}
