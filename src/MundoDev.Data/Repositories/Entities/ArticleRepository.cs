using Microsoft.EntityFrameworkCore;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Entities
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(MainDbContext db) : base(db) { }

        public override async Task<List<Article>> GetAll()
        {
            return await DbSet.AsNoTracking()
                .Include(a => a.Subject)
                .OrderByDescending(a => a.PublishDate)
                .ToListAsync();
        }

        public override async Task<Article> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(a => a.Subject)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Article?> GetBySlug(string slug)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(a => a.Slug == slug);
        }
    }
}
