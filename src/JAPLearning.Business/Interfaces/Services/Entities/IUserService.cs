using JAPLearning.Business.Interfaces.Services;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Interfaces.Services.Entities
{
    public interface IUserService : IBaseService<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
