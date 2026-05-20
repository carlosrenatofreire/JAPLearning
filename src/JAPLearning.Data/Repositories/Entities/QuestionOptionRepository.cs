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
                .OrderBy(o => o.Question.Name).ThenBy(o => o.Name)
                .ToListAsync();
        }

        public override async Task<QuestionOption> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(o => o.Question)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
