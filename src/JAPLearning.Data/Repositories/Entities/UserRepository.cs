using Microsoft.EntityFrameworkCore;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Data.Contexts;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Data.Repositories.Entities
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MainDbContext db) : base(db) { }

        public override async Task<List<User>> GetAll()
        {
            return await DbSet
                .AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.Team)
                .ToListAsync();
        }

        public override async Task<User> GetById(Guid id)
        {
            return await DbSet
                .AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await DbSet
                .AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.Team)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
