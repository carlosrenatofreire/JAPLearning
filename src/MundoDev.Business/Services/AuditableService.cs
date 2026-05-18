using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Auxiliaries;
using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Services
{
    public abstract class AuditableService<TEntity, TRepository> : BaseService<TEntity, TRepository>
        where TEntity : Entity
        where TRepository : IRepository<TEntity>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuditLogService _auditLog;

        protected AuditableService(
            IUnitOfWork uow,
            TRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator)
        {
            _httpContextAccessor = httpContextAccessor;
            _auditLog = auditLog;
        }

        private string GetCurrentUser() =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? "system";

        public override async Task<bool> AddAsync(TEntity entity)
        {
            var result = await base.AddAsync(entity);
            if (result)
                await _auditLog.LogAsync(GetCurrentUser(), "Criou", typeof(TEntity).Name,
                    JsonSerializer.Serialize(entity));
            return result;
        }

        public override async Task<bool> UpdateAsync(TEntity entity)
        {
            var result = await base.UpdateAsync(entity);
            if (result)
                await _auditLog.LogAsync(GetCurrentUser(), "Actualizou", typeof(TEntity).Name,
                    JsonSerializer.Serialize(entity));
            return result;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var result = await base.DeleteAsync(id);
            if (result)
                await _auditLog.LogAsync(GetCurrentUser(), "Eliminou", typeof(TEntity).Name,
                    JsonSerializer.Serialize(new { Id = id }));
            return result;
        }
    }
}
