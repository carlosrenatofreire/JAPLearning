using Microsoft.AspNetCore.Http;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Validations.Internals.Entities;

namespace JAPLearning.Business.Services.Entities
{
    public class QuestionOptionService : AuditableService<QuestionOption, IQuestionOptionRepository>, IQuestionOptionService
    {
        public QuestionOptionService(
            IUnitOfWork uow,
            IQuestionOptionRepository repository,
            INotificator notificator,
            IHttpContextAccessor httpContextAccessor,
            IAuditLogService auditLog)
            : base(uow, repository, notificator, httpContextAccessor, auditLog) { }

        public override async Task<bool> AddAsync(QuestionOption entity)
        {
            if (!await ValidateAsync(new QuestionOptionValidation(), entity)) return false;
            return await base.AddAsync(entity);
        }

        public override async Task<bool> UpdateAsync(QuestionOption entity)
        {
            if (!await ValidateAsync(new QuestionOptionValidation(), entity)) return false;
            return await base.UpdateAsync(entity);
        }
    }
}
