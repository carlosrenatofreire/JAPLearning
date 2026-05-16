using MundoDev.Business.Interfaces.Services;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Interfaces.Services.Entities
{
    public interface IArticleService : IBaseService<Article>
    {
        Task<Article?> GetBySlugAsync(string slug);
    }
}
