using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Interfaces.Internals.Entities
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmail(string email);
        /// <summary>Actualiza apenas LoginCount e LastLoginDate sem tocar noutros campos.</summary>
        Task<int> RecordLoginDirect(Guid userId, DateTime loginDate);
    }
}
