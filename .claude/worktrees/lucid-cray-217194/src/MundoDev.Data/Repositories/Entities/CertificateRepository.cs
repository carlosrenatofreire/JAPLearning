using Microsoft.EntityFrameworkCore;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Entities
{
    public class CertificateRepository : Repository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(MainDbContext db) : base(db) { }

        public override async Task<List<Certificate>> GetAll()
        {
            return await DbSet.AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Course)
                .OrderByDescending(c => c.CompletedDate)
                .ToListAsync();
        }

        public override async Task<Certificate> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
