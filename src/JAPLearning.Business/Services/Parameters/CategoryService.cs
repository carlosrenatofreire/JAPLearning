using JAPLearning.Business.Interfaces.Internals.Parameters;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Business.Validators;

namespace JAPLearning.Business.Services.Parameters
{
    public class CategoryService : BaseService<Category, ICategoryRepository>, ICategoryService
    {
        public CategoryService(IUnitOfWork uow, ICategoryRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }

        public override async Task<bool> AddAsync(Category entity)
        {
            if (!Validate(new CategoryValidator(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Category entity)
        {
            if (!Validate(new CategoryValidator(), entity)) return false;
            return await base.UpdateAsync(entity);
        }
    }
}
