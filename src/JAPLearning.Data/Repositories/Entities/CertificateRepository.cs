using Microsoft.EntityFrameworkCore;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Entities
{
    public class CertificateRepository : Repository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(MainDbContext db) : base(db) { }

        public override async Task<List<Certificate>> GetAll()
        {
            return await DbSet.AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Course).ThenInclude(co => co.Category)
                .OrderByDescending(c => c.CompletedDate)
                .ToListAsync();
        }

        public override async Task<Certificate> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Course).ThenInclude(co => co.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
