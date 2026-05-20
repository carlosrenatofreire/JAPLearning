using FluentValidation;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Validators
{
    public class LessonValidator : AbstractValidator<Lesson>
    {
        public LessonValidator()
        {
            RuleFor(l => l.CourseId)
                .NotEmpty().WithMessage("O curso da aula é obrigatório.");

            RuleFor(l => l.TopicId)
                .NotEmpty().WithMessage("O tópico da aula é obrigatório.");

            RuleFor(l => l.Name)
                .NotEmpty().WithMessage("O nome da aula é obrigatório.")
                .MaximumLength(250).WithMessage("O nome da aula deve ter no máximo 250 caracteres.");

            RuleFor(l => l.Order)
                .GreaterThan(0).WithMessage("A ordem da aula deve ser maior que zero.");

            RuleFor(l => l.Video)
                .MaximumLength(500).WithMessage("O URL do vídeo deve ter no máximo 500 caracteres.")
                .When(l => l.Video != null);
        }
    }
}
