using FluentValidation;
using MundoDev.Mvc.ViewModels.Entities;

namespace MundoDev.Mvc.Validators.Entities
{
    public class ArticleValidator : AbstractValidator<ArticleViewModel>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(x => x.SubjectId)
                .NotEmpty().WithMessage("Subject is required.");

            RuleFor(x => x.Slug)
                .MaximumLength(250).WithMessage("Slug must not exceed 250 characters.");
        }
    }
}
