using Microsoft.EntityFrameworkCore;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Entities
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
