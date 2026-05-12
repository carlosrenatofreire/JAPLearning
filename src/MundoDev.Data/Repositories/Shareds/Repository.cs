using Microsoft.EntityFrameworkCore;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Models.Shareds;
using MundoDev.Data.Contexts;
using System.Linq.Expressions;

namespace MundoDev.Data.Repositories.Shareds
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly MainDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(MainDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual async Task Remove(Guid id)
        {
            var entity = new TEntity { Id = id };
            DbSet.Remove(entity);
        }

        public virtual async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }

}
