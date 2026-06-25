using FluentValidation;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Business.Validations.Internals.Parameters
{
    public class LevelValidation : AbstractValidator<Level>
    {
        public LevelValidation()
        {
            RuleFor(l => l.Name)
                .NotEmpty().WithMessage("O nome do nível é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do nível deve ter no máximo 100 caracteres.");

            RuleFor(l => l.Description)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.")
                .When(l => !string.IsNullOrEmpty(l.Description));
        }
    }
}
