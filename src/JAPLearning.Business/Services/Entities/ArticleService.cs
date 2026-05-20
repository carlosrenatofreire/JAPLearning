using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Services.Entities
{
    public class ArticleService : BaseService<Article, IArticleRepository>, IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IUnitOfWork uow, IArticleRepository repository, INotificator notificator)
            : base(uow, repository, notificator)
        {
            _articleRepository = repository;
        }

        public async Task<Article?> GetBySlugAsync(string slug) =>
            await _articleRepository.GetBySlug(slug);
    }
}
