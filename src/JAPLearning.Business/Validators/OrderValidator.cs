using FluentValidation;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(o => o.UserId)
                .NotEmpty().WithMessage("O utilizador da encomenda é obrigatório.");

            RuleFor(o => o.PlanId)
                .NotEmpty().WithMessage("O plano da encomenda é obrigatório.");

            RuleFor(o => o.StatusId)
                .NotEmpty().WithMessage("O estado da encomenda é obrigatório.");

            RuleFor(o => o.Value)
                .GreaterThanOrEqualTo(0).WithMessage("O valor da encomenda não pode ser negativo.");
        }
    }
}
