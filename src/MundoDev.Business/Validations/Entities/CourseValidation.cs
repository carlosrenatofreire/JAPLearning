using FluentValidation;
using MundoDev.Business.Models.Domains.Entities;

namespace MundoDev.Business.Validations.Entities
{
    public class CourseValidation : AbstractValidator<Course>
    {
        public CourseValidation()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required.");

            RuleFor(x => x.TeacherId)
                .NotEmpty().WithMessage("Teacher is required.");

            RuleFor(x => x.LevelId)
                .NotEmpty().WithMessage("Level is required.");
        }
    }
}
