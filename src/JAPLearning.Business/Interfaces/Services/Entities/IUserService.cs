using JAPLearning.Business.Interfaces.Services;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Interfaces.Services.Entities
{
    public interface IUserService : IBaseService<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> RecordLoginAsync(Guid userId);
        /// <param name="newPasswordHash">Nova senha já com BCrypt hash aplicado</param>
        Task<bool> ChangePasswordAsync(Guid userId, string newPasswordHash);
    }
}
