using Microsoft.EntityFrameworkCore;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Entities
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(MainDbContext db) : base(db) { }

        public override async Task<List<Question>> GetAll()
        {
            return await DbSet.AsNoTracking()
                .Include(q => q.Lesson)
                    .ThenInclude(l => l.Course)
                        .ThenInclude(c => c.Category)
                            .ThenInclude(cat => cat.Team)
                .Include(q => q.Options)
                .OrderBy(q => q.Lesson.Course.Category.Team.Name)
                    .ThenBy(q => q.Lesson.Course.Title)
                    .ThenBy(q => q.Lesson.Name)
                    .ThenBy(q => q.Name)
                .ToListAsync();
        }

        public override async Task<Question> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(q => q.Lesson)
                    .ThenInclude(l => l.Course)
                        .ThenInclude(c => c.Category)
                            .ThenInclude(cat => cat.Team)
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}
