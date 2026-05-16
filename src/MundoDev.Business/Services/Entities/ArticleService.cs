using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Services.Entities
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
