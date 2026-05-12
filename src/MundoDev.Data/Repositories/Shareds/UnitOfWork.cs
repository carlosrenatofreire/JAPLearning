using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Data.Contexts;

namespace MundoDev.Data.Repositories.Shareds
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MainDbContext _context;

        public UnitOfWork(MainDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
