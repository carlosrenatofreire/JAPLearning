using JAPLearning.Business.Interfaces.Services;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Interfaces.Services.Entities
{
    public interface IArticleService : IBaseService<Article>
    {
        Task<Article?> GetBySlugAsync(string slug);
    }
}
