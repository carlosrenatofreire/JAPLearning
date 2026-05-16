using MundoDev.Business.Interfaces.Internals.Auxiliaries;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Auxiliaries;
using MundoDev.Business.Models.Domains.Auxiliaries;

namespace MundoDev.Business.Services.Auxiliaries
{
    public class AuditLogService : BaseService<AuditLog, IAuditLogRepository>, IAuditLogService
    {
        public AuditLogService(IUnitOfWork uow, IAuditLogRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }
    }
}
