using FluentValidation;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Validators
{
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(c => c.CategoryId)
                .NotEmpty().WithMessage("A categoria do curso é obrigatória.");

            RuleFor(c => c.TeacherId)
                .NotEmpty().WithMessage("O professor do curso é obrigatório.");

            RuleFor(c => c.LevelId)
                .NotEmpty().WithMessage("O nível do curso é obrigatório.");

            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("O título do curso é obrigatório.")
                .MaximumLength(250).WithMessage("O título do curso deve ter no máximo 250 caracteres.");

            RuleFor(c => c.Subtitle)
                .MaximumLength(500).WithMessage("O subtítulo do curso deve ter no máximo 500 caracteres.")
                .When(c => c.Subtitle != null);

            RuleFor(c => c.PassingScore)
                .InclusiveBetween(0, 100).WithMessage("A nota de aprovação deve estar entre 0 e 100.");
        }
    }
}
