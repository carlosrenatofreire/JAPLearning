using JAPLearning.Business.Interfaces.Services;
using JAPLearning.Business.Models.Domains.Auxiliaries;

namespace JAPLearning.Business.Interfaces.Services.Auxiliaries
{
    public interface IAuditLogService : IBaseService<AuditLog>
    {
        Task LogAsync(string createdBy, string action, string entityName, string? json = null);
    }
}
