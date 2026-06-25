using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Models.Shareds;

namespace JAPLearning.Business.Services
{
    public abstract class AuditableService<TEntity, TRepository> : BaseService<TEntity, TRepository>
        where TEntity : Entity
        where TRepository : IRepository<TEntity>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuditLogService     _auditLog;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            ReferenceHandler       = ReferenceHandler.IgnoreCycles,
            WriteIndented          = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder                = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

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
            SetAuditLog(auditLog); // permite que BaseService.Validate() registe avisos
        }

        private string GetCurrentUser() =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? "system";

        protected override string GetAuditUser() => GetCurrentUser();

        private static string SafeSerialize(object obj)
        {
            try   { return JsonSerializer.Serialize(obj, _jsonOptions); }
            catch { return "{}"; }
        }

        public override async Task<bool> AddAsync(TEntity entity)
        {
            var result = await base.AddAsync(entity);
            if (result)
                await TryLog(GetCurrentUser(), "Created", typeof(TEntity).Name, SafeSerialize(entity));
            return result;
        }

        public override async Task<bool> UpdateAsync(TEntity entity)
        {
            var result = await base.UpdateAsync(entity);
            if (result)
                await TryLog(GetCurrentUser(), "Updated", typeof(TEntity).Name, SafeSerialize(entity));
            return result;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var result = await base.DeleteAsync(id);
            if (result)
                await TryLog(GetCurrentUser(), "Deleted", typeof(TEntity).Name, SafeSerialize(new { Id = id }));
            return result;
        }

        private async Task TryLog(string user, string action, string entity, string json)
        {
            try
            {
                await _auditLog.LogInfoAsync(user, action, entity, json);
            }
            catch
            {
                // Falha no log não deve interromper a operação principal
            }
        }
    }
}
