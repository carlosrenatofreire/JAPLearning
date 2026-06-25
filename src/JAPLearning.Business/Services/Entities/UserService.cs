using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Validations.Internals.Entities;

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

        public override async Task<bool> AddAsync(User entity)
        {
            if (!await ValidateAsync(new UserValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(User entity)
        {
            if (!await ValidateAsync(new UserValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _userRepository.GetByEmail(email);

        /// <summary>
        /// Incrementa LoginCount, actualiza LastLoginDate e limpa MustChangePassword se necessário.
        /// Chamado após login bem sucedido de Aluno.
        /// </summary>
        public async Task<bool> RecordLoginAsync(Guid userId)
        {
            // Usa ExecuteUpdateAsync directo para NÃO sobrescrever MustChangePassword
            // (evita carregar + Update completo da entidade que resetaria o flag)
            var count = await _userRepository.RecordLoginDirect(userId, DateTime.UtcNow);
            return count > 0;
        }

        /// <summary>
        /// Altera a senha do utilizador e limpa o flag MustChangePassword.
        /// </summary>
        public async Task<bool> ChangePasswordAsync(Guid userId, string newPasswordHash)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                _notificator.AddNotification("Utilizador não encontrado.");
                return false;
            }

            // Hash já aplicado pelo controller (BCrypt está só no projecto Mvc)
            user.Password           = newPasswordHash;
            user.MustChangePassword = false;
            user.ChangedDate        = DateTime.UtcNow;

            await _userRepository.Update(user);
            return await _uow.Commit();
        }
    }
}
