using FluentValidation;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Business.Validations.Internals.Parameters
{
    public class TeamValidation : AbstractValidator<Team>
    {
        public TeamValidation()
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("O nome da equipa é obrigatório.")
                .MaximumLength(100).WithMessage("O nome da equipa deve ter no máximo 100 caracteres.");

            RuleFor(t => t.Description)
                .MaximumLength(1000).WithMessage("A descrição deve ter no máximo 1000 caracteres.")
                .When(t => t.Description != null);
        }
    }
}
