using Microsoft.EntityFrameworkCore;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Entities
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(MainDbContext db) : base(db) { }

        public override async Task<List<Lesson>> GetAll()
        {
            return await DbSet.AsNoTracking()
                .Include(l => l.Course)
                .Include(l => l.Topic)
                .OrderBy(l => l.Course.Title).ThenBy(l => l.Order)
                .ToListAsync();
        }

        public override async Task<Lesson> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(l => l.Course)
                .Include(l => l.Topic)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
