using FluentValidation;
using JAPLearning.Business.Models.Domains.Auxiliaries;

namespace JAPLearning.Business.Validations.Internals.Auxiliaries
{
    public class AuditLogValidation : AbstractValidator<AuditLog>
    {
        public AuditLogValidation()
        {
            RuleFor(a => a.CreatedBy)
                .NotEmpty().WithMessage("O utilizador do registo de auditoria é obrigatório.")
                .MaximumLength(200).WithMessage("O utilizador deve ter no máximo 200 caracteres.");

            RuleFor(a => a.Action)
                .MaximumLength(100).WithMessage("A acção deve ter no máximo 100 caracteres.")
                .When(a => !string.IsNullOrEmpty(a.Action));

            RuleFor(a => a.EntityName)
                .MaximumLength(100).WithMessage("O nome da entidade deve ter no máximo 100 caracteres.")
                .When(a => !string.IsNullOrEmpty(a.EntityName));

            RuleFor(a => a.Message)
                .MaximumLength(2000).WithMessage("A mensagem deve ter no máximo 2000 caracteres.")
                .When(a => !string.IsNullOrEmpty(a.Message));
        }
    }
}
