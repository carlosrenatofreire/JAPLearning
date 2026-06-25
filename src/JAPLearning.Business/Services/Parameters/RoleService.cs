using JAPLearning.Business.Interfaces.Internals.Parameters;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Business.Validations.Internals.Parameters;

namespace JAPLearning.Business.Services.Parameters
{
    public class RoleService : BaseService<Role, IRoleRepository>, IRoleService
    {
        public RoleService(IUnitOfWork uow, IRoleRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }

        public override async Task<bool> AddAsync(Role entity)
        {
            if (!await ValidateAsync(new RoleValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Role entity)
        {
            if (!await ValidateAsync(new RoleValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }
    }
}
