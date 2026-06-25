using FluentValidation;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Validators
{
    public class TopicValidator : AbstractValidator<Topic>
    {
        public TopicValidator()
        {
            RuleFor(t => t.CourseId)
                .NotEmpty().WithMessage("O curso do tópico é obrigatório.");

            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("O nome do tópico é obrigatório.")
                .MaximumLength(250).WithMessage("O nome do tópico deve ter no máximo 250 caracteres.");

            RuleFor(t => t.Order)
                .GreaterThan(0).WithMessage("A ordem do tópico deve ser maior que zero.");
        }
    }
}
