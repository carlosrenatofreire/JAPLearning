using MundoDev.Business.Interfaces.Services;
using MundoDev.Business.Models.Domains.Auxiliaries;

namespace MundoDev.Business.Interfaces.Services.Auxiliaries
{
    public interface IAuditLogService : IBaseService<AuditLog>
    {
        Task LogAsync(string createdBy, string action, string entityName, string? json = null);
    }
}
