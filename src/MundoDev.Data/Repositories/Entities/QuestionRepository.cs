using Microsoft.EntityFrameworkCore;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Entities
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(MainDbContext db) : base(db) { }

        public override async Task<List<Question>> GetAll()
        {
            return await DbSet.AsNoTracking()
                .Include(q => q.Lesson)
                .Include(q => q.Options)
                .OrderBy(q => q.Lesson.Name).ThenBy(q => q.Name)
                .ToListAsync();
        }

        public override async Task<Question> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(q => q.Lesson)
                .Include(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}
