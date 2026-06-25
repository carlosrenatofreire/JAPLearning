using MundoDev.Business.Interfaces.Services;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Interfaces.Services.Entities
{
    public interface IUserService : IBaseService<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
