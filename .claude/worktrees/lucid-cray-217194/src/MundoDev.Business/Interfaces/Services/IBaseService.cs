using MundoDev.Business.Models.Shareds;

namespace MundoDev.Business.Interfaces.Services
{
    public interface IBaseService<TEntity> where TEntity : Entity
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
