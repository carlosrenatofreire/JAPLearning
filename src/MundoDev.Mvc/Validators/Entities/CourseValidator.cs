using FluentValidation;
using MundoDev.Mvc.ViewModels.Entities;

namespace MundoDev.Mvc.Validators.Entities
{
    public class CourseValidator : AbstractValidator<CourseViewModel>
    {
        public CourseValidator()
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
