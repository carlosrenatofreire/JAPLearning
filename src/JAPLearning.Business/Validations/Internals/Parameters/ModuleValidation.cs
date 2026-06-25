using FluentValidation;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Business.Validations.Internals.Parameters
{
    public class ModuleValidation : AbstractValidator<Module>
    {
        public ModuleValidation()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("O nome do módulo é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do módulo deve ter no máximo 100 caracteres.");

            RuleFor(m => m.Description)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.")
                .When(m => !string.IsNullOrEmpty(m.Description));
        }
    }
}
