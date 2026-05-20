using Microsoft.EntityFrameworkCore;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Entities
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(MainDbContext db) : base(db) { }

        public override async Task<List<Course>> GetAll()
        {
            return await DbSet.AsNoTracking()
                .Include(c => c.Category)
                .Include(c => c.Teacher)
                .Include(c => c.Level)
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public override async Task<Course> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(c => c.Category)
                .Include(c => c.Teacher)
                .Include(c => c.Level)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
