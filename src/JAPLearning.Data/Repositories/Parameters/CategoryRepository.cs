using Microsoft.EntityFrameworkCore;
using JAPLearning.Business.Interfaces.Internals.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Parameters
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(MainDbContext db) : base(db) { }

        public override async Task<List<Category>> GetAll()
        {
            return await DbSet
                .AsNoTracking()
                .Include(c => c.Team)
                .ToListAsync();
        }

        public override async Task<Category> GetById(Guid id)
        {
            return await DbSet
                .AsNoTracking()
                .Include(c => c.Team)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
