using Microsoft.EntityFrameworkCore;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Entities
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        public TopicRepository(MainDbContext db) : base(db) { }

        public override async Task<List<Topic>> GetAll()
        {
            return await DbSet.AsNoTracking()
                .Include(t => t.Course)
                .OrderBy(t => t.Course.Title).ThenBy(t => t.Order)
                .ToListAsync();
        }

        public override async Task<Topic> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(t => t.Course)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
