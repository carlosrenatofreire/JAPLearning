using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Services.Entities
{
    public class ArticleService : AuditableService<Article, IArticleRepository>, IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(
            IUnitOfWork uow,
            IArticleRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog)
        {
            _articleRepository = repository;
        }

        public async Task<Article?> GetBySlugAsync(string slug) =>
            await _articleRepository.GetBySlug(slug);
    }
}
