using MundoDev.Business.Interfaces.Internals.Auxiliaries;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Auxiliaries;
using MundoDev.Business.Models.Domains.Auxiliaries;
using MundoDev.Business.Models.Enums;

namespace MundoDev.Business.Services.Auxiliaries
{
    public class AuditLogService : BaseService<AuditLog, IAuditLogRepository>, IAuditLogService
    {
        public AuditLogService(IUnitOfWork uow, IAuditLogRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }

        public async Task LogAsync(string createdBy, string action, string entityName, string? json = null)
        {
            var log = new AuditLog
            {
                LogLevel    = LogType.Info,
                CreatedDate = DateTime.UtcNow,
                CreatedBy   = createdBy,
                Message     = $"{action} {entityName}",
                Json        = json
            };
            await _repository.Add(log);
            await _uow.Commit();
        }
    }
}
