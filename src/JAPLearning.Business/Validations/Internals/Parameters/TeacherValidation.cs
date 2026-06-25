using FluentValidation;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Business.Validations.Internals.Parameters
{
    public class TeacherValidation : AbstractValidator<Teacher>
    {
        public TeacherValidation()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("O nome do professor é obrigatório.")
                .MinimumLength(3).WithMessage("O nome do professor deve ter no mínimo 3 caracteres.")
                .MaximumLength(150).WithMessage("O nome do professor deve ter no máximo 150 caracteres.");

            RuleFor(t => t.Description)
                .MaximumLength(5000).WithMessage("A descrição deve ter no máximo 5000 caracteres.")
                .When(t => !string.IsNullOrEmpty(t.Description));
        }
    }
}
