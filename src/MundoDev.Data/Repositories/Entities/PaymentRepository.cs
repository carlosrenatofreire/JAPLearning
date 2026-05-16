using Microsoft.EntityFrameworkCore;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Entities
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(MainDbContext db) : base(db) { }

        public override async Task<List<Payment>> GetAll()
        {
            return await DbSet.AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.Order)
                .OrderByDescending(p => p.PaidDate)
                .ToListAsync();
        }

        public override async Task<Payment> GetById(Guid id)
        {
            return await DbSet.AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
