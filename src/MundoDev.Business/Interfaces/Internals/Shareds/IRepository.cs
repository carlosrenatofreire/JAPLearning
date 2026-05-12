using MundoDev.Business.Models.Shareds;
using System.Linq.Expressions;

namespace MundoDev.Business.Interfaces.Internals.Shareds
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        /* Read methods */
        Task<List<TEntity>> GetAll();
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);


        /* Persistence methods (write) */
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Remove(Guid id);
        Task<int> SaveChanges();
    }
}
