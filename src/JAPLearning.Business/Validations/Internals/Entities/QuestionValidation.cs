using FluentValidation;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Validations.Internals.Entities
{
    public class QuestionValidation : AbstractValidator<Question>
    {
        public QuestionValidation()
        {
            RuleFor(q => q.LessonId)
                .NotEmpty().WithMessage("A aula da questão é obrigatória.");

            RuleFor(q => q.Name)
                .NotEmpty().WithMessage("O enunciado da questão é obrigatório.")
                .MinimumLength(5).WithMessage("O enunciado deve ter no mínimo 5 caracteres.")
                .MaximumLength(500).WithMessage("O enunciado deve ter no máximo 500 caracteres.");

            RuleFor(q => q.Description)
                .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres.")
                .When(q => !string.IsNullOrEmpty(q.Description));
        }
    }
}
