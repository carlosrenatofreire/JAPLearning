using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Services.Entities
{
    public class UserService : AuditableService<User, IUserRepository>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            IUnitOfWork uow,
            IUserRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog)
        {
            _userRepository = repository;
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _userRepository.GetByEmail(email);
    }
}
