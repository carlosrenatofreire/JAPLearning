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

        public async Task<Article?> GetBySlug(string slug)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(a => a.Slug == slug);
        }
    }
}
