using FluentValidation;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Business.Validations.Internals.Parameters
{
    public class RoleValidation : AbstractValidator<Role>
    {
        public RoleValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do perfil é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do perfil deve ter no máximo 100 caracteres.");
        }
    }
}
