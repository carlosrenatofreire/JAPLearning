using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Validations.Internals.Entities;

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

        public override async Task<bool> AddAsync(Article entity)
        {
            if (!await ValidateAsync(new ArticleValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Article entity)
        {
            if (!await ValidateAsync(new ArticleValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }

        public async Task<Article?> GetBySlugAsync(string slug) =>
            await _articleRepository.GetBySlug(slug);
    }
}
