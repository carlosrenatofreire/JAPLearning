using FluentValidation;
using JAPLearning.Business.Models.Domains.Parameters;

namespace JAPLearning.Business.Validations.Internals.Parameters
{
    public class CategoryValidation : AbstractValidator<Category>
    {
        public CategoryValidation()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
                .MaximumLength(150).WithMessage("O nome da categoria deve ter no máximo 150 caracteres.");

            RuleFor(c => c.Subtitle)
                .MaximumLength(250).WithMessage("O subtítulo da categoria deve ter no máximo 250 caracteres.")
                .When(c => c.Subtitle != null);

            RuleFor(c => c.Description)
                .MaximumLength(1000).WithMessage("A descrição da categoria deve ter no máximo 1000 caracteres.")
                .When(c => c.Description != null);
        }
    }
}
