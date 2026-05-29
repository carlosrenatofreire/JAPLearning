using JAPLearning.Business.Interfaces.Services;
using JAPLearning.Business.Models.Domains.Auxiliaries;

namespace JAPLearning.Business.Interfaces.Services.Auxiliaries
{
    public interface IAuditLogService : IBaseService<AuditLog>
    {
        Task LogInfoAsync(string createdBy, string action, string entityName, string? json = null);
        Task LogErrorAsync(string createdBy, string message, int? httpStatusCode = null, string? stackTrace = null);
    }
}
