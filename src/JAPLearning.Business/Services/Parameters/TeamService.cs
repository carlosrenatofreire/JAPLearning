using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Parameters;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Business.Validators;

namespace JAPLearning.Business.Services.Parameters
{
    public class TeamService : AuditableService<Team, ITeamRepository>, ITeamService
    {
        public TeamService(
            IUnitOfWork uow,
            ITeamRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog) { }

        public override async Task<bool> AddAsync(Team entity)
        {
            if (!Validate(new TeamValidator(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(Team entity)
        {
            if (!Validate(new TeamValidator(), entity)) return false;
            return await base.UpdateAsync(entity);
        }
    }
}
