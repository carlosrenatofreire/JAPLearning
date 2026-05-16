using MundoDev.Business.Interfaces.Internals.Parameters;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Parameters;
using MundoDev.Business.Models.Domains.Parameters;

namespace MundoDev.Business.Services.Parameters
{
    public class CategoryService : BaseService<Category, ICategoryRepository>, ICategoryService
    {
        public CategoryService(IUnitOfWork uow, ICategoryRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }
    }
}
