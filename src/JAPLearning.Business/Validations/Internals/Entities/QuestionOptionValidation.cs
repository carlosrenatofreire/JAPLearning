using FluentValidation;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Validations.Internals.Entities
{
    public class QuestionOptionValidation : AbstractValidator<QuestionOption>
    {
        public QuestionOptionValidation()
        {
            RuleFor(o => o.QuestionId)
                .NotEmpty().WithMessage("A questão da opção é obrigatória.");

            RuleFor(o => o.Name)
                .NotEmpty().WithMessage("O texto da opção é obrigatório.")
                .MaximumLength(500).WithMessage("O texto da opção deve ter no máximo 500 caracteres.");

            RuleFor(o => o.Description)
                .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres.")
                .When(o => !string.IsNullOrEmpty(o.Description));
        }
    }
}
