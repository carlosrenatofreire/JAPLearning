using FluentValidation;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Models.Shareds;
using JAPLearning.Business.Notifications;

namespace JAPLearning.Business.Services
{
    public abstract class BaseService<TEntity, TRepository> : IBaseService<TEntity>
        where TEntity : Entity
        where TRepository : IRepository<TEntity>
    {
        protected readonly IUnitOfWork _uow;
        protected readonly TRepository _repository;
        protected readonly INotificator _notificator;
        private IAuditLogService? _auditLog;

        protected BaseService(IUnitOfWork uow, TRepository repository, INotificator notificator)
        {
            _uow = uow;
            _repository = repository;
            _notificator = notificator;
        }

        /// <summary>
        /// Injectado opcionalmente pelos serviços que herdam de AuditableService.
        /// Permite que BaseService registe avisos de validação sem criar dependência circular.
        /// </summary>
        protected void SetAuditLog(IAuditLogService auditLog) => _auditLog = auditLog;

        // Sobreposto em AuditableService para devolver o utilizador autenticado
        protected virtual string GetAuditUser() => "system";

        protected async Task<bool> ValidateAsync<TV>(TV validator, TEntity entity) where TV : AbstractValidator<TEntity>
        {
            var result = validator.Validate(entity);
            if (result.IsValid) return true;

            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();

            foreach (var error in errors)
                _notificator.AddNotification(error);

            // Regista aviso de validação na auditoria (awaited — evita concorrência no DbContext)
            if (_auditLog != null)
            {
                var message = string.Join(" | ", errors);
                try { await _auditLog.LogWarningAsync(GetAuditUser(), "ValidationFailed", typeof(TEntity).Name, message); }
                catch { /* falha no log não pode interromper o fluxo */ }
            }

            return false;
        }

        public virtual async Task<List<TEntity>> GetAllAsync() =>
            await _repository.GetAll();

        public virtual async Task<TEntity?> GetByIdAsync(Guid id) =>
            await _repository.GetById(id);

        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            await _repository.Add(entity);
            return await _uow.Commit();
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            await _repository.Update(entity);
            return await _uow.Commit();
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            await _repository.Remove(id);
            return await _uow.Commit();
        }
    }
}
