using Microsoft.EntityFrameworkCore;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Entities
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(MainDbContext db) : base(db) { }

        public override async Task<List<Order>> GetAll()
        {
            return await DbSet.AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.Plan)
                .Include(o => o.Status)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
        }

        public override async Task<Order> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.Plan)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
