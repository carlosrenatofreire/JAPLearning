using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Parameters;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Business.Validations.Internals.Parameters;

namespace JAPLearning.Business.Services.Parameters
{
    public class PermissionService : AuditableService<Permission, IPermissionRepository>, IPermissionService
    {
        public PermissionService(
            IUnitOfWork uow,
            IPermissionRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog) { }

        public override async Task<bool> AddAsync(Permission entity)
        {
            if (!await ValidateAsync(new PermissionValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Permission entity)
        {
            if (!await ValidateAsync(new PermissionValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }
    }
}
