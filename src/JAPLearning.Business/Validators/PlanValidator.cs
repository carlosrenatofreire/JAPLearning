using FluentValidation;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Validators
{
    public class PlanValidator : AbstractValidator<Plan>
    {
        public PlanValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("O nome do plano é obrigatório.")
                .MaximumLength(150).WithMessage("O nome do plano deve ter no máximo 150 caracteres.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("O preço do plano deve ser maior que zero.")
                .When(p => p.Price.HasValue);

            RuleFor(p => p.Months)
                .GreaterThan(0).WithMessage("O número de meses deve ser maior que zero.")
                .When(p => p.Months.HasValue);

            RuleFor(p => p.DiscountPercent)
                .InclusiveBetween(0, 100).WithMessage("O percentual de desconto deve estar entre 0 e 100.")
                .When(p => p.DiscountPercent.HasValue);
        }
    }
}
