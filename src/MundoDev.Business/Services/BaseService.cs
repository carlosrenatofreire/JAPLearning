using FluentValidation;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services;
using MundoDev.Business.Models.Shareds;
using MundoDev.Business.Notifications;

namespace MundoDev.Business.Services
{
    public abstract class BaseService<TEntity, TRepository> : IBaseService<TEntity>
        where TEntity : Entity
        where TRepository : IRepository<TEntity>
    {
        protected readonly IUnitOfWork _uow;
        protected readonly TRepository _repository;
        protected readonly INotificator _notificator;

        protected BaseService(IUnitOfWork uow, TRepository repository, INotificator notificator)
        {
            _uow = uow;
            _repository = repository;
            _notificator = notificator;
        }

        protected bool Validate<TV>(TV validator, TEntity entity) where TV : AbstractValidator<TEntity>
        {
            var result = validator.Validate(entity);
            if (result.IsValid) return true;

            foreach (var error in result.Errors)
                _notificator.AddNotification(error.ErrorMessage);

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
