using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Entities
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(MainDbContext db) : base(db) { }
    }
}
