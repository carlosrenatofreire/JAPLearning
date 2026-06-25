using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Parameters;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Business.Validations.Internals.Parameters;

namespace JAPLearning.Business.Services.Parameters
{
    public class CategoryService : AuditableService<Category, ICategoryRepository>, ICategoryService
    {
        public CategoryService(
            IUnitOfWork uow,
            ICategoryRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog) { }

        public override async Task<bool> AddAsync(Category entity)
        {
            if (!await ValidateAsync(new CategoryValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Category entity)
        {
            if (!await ValidateAsync(new CategoryValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }
    }
}
