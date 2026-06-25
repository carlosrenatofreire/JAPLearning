using FluentValidation;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Business.Validations.Internals.Parameters
{
    public class SubjectValidation : AbstractValidator<Subject>
    {
        public SubjectValidation()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("O nome do assunto é obrigatório.")
                .MaximumLength(150).WithMessage("O nome do assunto deve ter no máximo 150 caracteres.");

            RuleFor(s => s.Description)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.")
                .When(s => !string.IsNullOrEmpty(s.Description));
        }
    }
}
