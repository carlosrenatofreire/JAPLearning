using JAPLearning.Business.Interfaces.Internals.Auxiliaries;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Models.Domains.Auxiliaries;
using JAPLearning.Business.Models.Enums;
using JAPLearning.Business.Validations.Internals.Auxiliaries;

namespace JAPLearning.Business.Services.Auxiliaries
{
    public class AuditLogService : BaseService<AuditLog, IAuditLogRepository>, IAuditLogService
    {
        public AuditLogService(IUnitOfWork uow, IAuditLogRepository repository, INotificator notificator)
            : base(uow, repository, notificator) { }

        public override async Task<bool> AddAsync(AuditLog entity)
        {
            if (!await ValidateAsync(new AuditLogValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(AuditLog entity)
        {
            if (!await ValidateAsync(new AuditLogValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }

        public async Task LogInfoAsync(string createdBy, string action, string entityName, string? json = null)
        {
            var log = new AuditLog
            {
                LogLevel    = LogType.Info,
                CreatedDate = DateTime.UtcNow,
                CreatedBy   = createdBy,
                Action      = action,
                EntityName  = entityName,
                Message     = $"{action} {entityName}",
                Json        = json
            };
            await _repository.Add(log);
            await _uow.Commit();
        }

        public async Task LogWarningAsync(string createdBy, string action, string entityName, string message)
        {
            var log = new AuditLog
            {
                LogLevel    = LogType.Warning,
                CreatedDate = DateTime.UtcNow,
                CreatedBy   = createdBy,
                Action      = action,
                EntityName  = entityName,
                Message     = message
            };
            await _repository.Add(log);
            await _uow.Commit();
        }

        public async Task LogErrorAsync(string createdBy, string message, int? httpStatusCode = null, string? stackTrace = null)
        {
            var logLevel = httpStatusCode switch
            {
                401 or 403      => LogType.Warning,
                404             => LogType.Warning,
                >= 500          => LogType.Error,
                _               => LogType.Error
            };

            var log = new AuditLog
            {
                LogLevel       = logLevel,
                CreatedDate    = DateTime.UtcNow,
                CreatedBy      = createdBy,
                Action         = httpStatusCode.HasValue ? $"Http{httpStatusCode}" : "Error",
                HttpStatusCode = httpStatusCode,
                Message        = message,
                StackTrace     = stackTrace
            };
            await _repository.Add(log);
            await _uow.Commit();
        }
    }
}
