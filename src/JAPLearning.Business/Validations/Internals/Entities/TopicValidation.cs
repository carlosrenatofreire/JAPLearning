using FluentValidation;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Validations.Internals.Entities
{
    public class TopicValidation : AbstractValidator<Topic>
    {
        public TopicValidation()
        {
            RuleFor(t => t.CourseId)
                .NotEmpty().WithMessage("A formação do tópico é obrigatória.");

            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("O nome do tópico é obrigatório.")
                .MinimumLength(3).WithMessage("O nome do tópico deve ter no mínimo 3 caracteres.")
                .MaximumLength(100).WithMessage("O nome do tópico deve ter no máximo 100 caracteres.");

            RuleFor(t => t.Description)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.")
                .When(t => !string.IsNullOrEmpty(t.Description));

            RuleFor(t => t.Order)
                .GreaterThan(0).WithMessage("A ordem do tópico deve ser maior que zero.");
        }
    }
}
