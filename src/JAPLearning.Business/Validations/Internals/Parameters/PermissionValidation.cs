using FluentValidation;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Business.Validations.Internals.Parameters
{
    public class PermissionValidation : AbstractValidator<Permission>
    {
        public PermissionValidation()
        {
            RuleFor(p => p.ModuleId)
                .NotEmpty().WithMessage("O módulo da permissão é obrigatório.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("O nome da permissão é obrigatório.")
                .MaximumLength(100).WithMessage("O nome da permissão deve ter no máximo 100 caracteres.");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.")
                .When(p => !string.IsNullOrEmpty(p.Description));
        }
    }
}
