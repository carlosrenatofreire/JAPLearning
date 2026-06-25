using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Services.Entities
{
    public class UserService : BaseService<User, IUserRepository>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork uow, IUserRepository repository, INotificator notificator)
            : base(uow, repository, notificator)
        {
            _userRepository = repository;
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _userRepository.GetByEmail(email);
    }
}
