using Microsoft.EntityFrameworkCore;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Entities
{
    public class QuestionOptionRepository : Repository<QuestionOption>, IQuestionOptionRepository
    {
        public QuestionOptionRepository(MainDbContext db) : base(db) { }

        public override async Task<List<QuestionOption>> GetAll()
        {
            return await DbSet.AsNoTracking()
                .Include(o => o.Question)
                    .ThenInclude(q => q.Lesson)
                        .ThenInclude(l => l.Course)
                            .ThenInclude(c => c.Category)
                                .ThenInclude(cat => cat.Team)
                .OrderBy(o => o.Question.Lesson.Course.Category.Team.Name)
                    .ThenBy(o => o.Question.Lesson.Course.Title)
                    .ThenBy(o => o.Question.Lesson.Name)
                    .ThenBy(o => o.Question.Name)
                    .ThenBy(o => o.Name)
                .ToListAsync();
        }

        public override async Task<QuestionOption> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(o => o.Question)
                    .ThenInclude(q => q.Lesson)
                        .ThenInclude(l => l.Course)
                            .ThenInclude(c => c.Category)
                                .ThenInclude(cat => cat.Team)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
