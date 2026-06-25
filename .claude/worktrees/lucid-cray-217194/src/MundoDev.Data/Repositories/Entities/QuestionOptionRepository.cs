using Microsoft.EntityFrameworkCore;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Entities
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
