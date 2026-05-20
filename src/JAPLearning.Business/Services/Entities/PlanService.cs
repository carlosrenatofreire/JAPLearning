using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Validators;

namespace JAPLearning.Business.Services.Entities
{
    public class PlanService : AuditableService<Plan, IPlanRepository>, IPlanService
    {
        public PlanService(
            IUnitOfWork uow,
            IPlanRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog) { }

        public override async Task<bool> AddAsync(Plan entity)
        {
            if (!Validate(new PlanValidator(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Plan entity)
        {
            if (!Validate(new PlanValidator(), entity)) return false;
            return await base.UpdateAsync(entity);
        }
    }
}
