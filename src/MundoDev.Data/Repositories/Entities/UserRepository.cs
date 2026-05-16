using Microsoft.EntityFrameworkCore;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Entities
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MainDbContext db) : base(db) { }

        public async Task<User?> GetByEmail(string email)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
