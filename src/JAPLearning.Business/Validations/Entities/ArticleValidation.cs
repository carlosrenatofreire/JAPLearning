using FluentValidation;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Validations.Entities
{
    public class ArticleValidation : AbstractValidator<Article>
    {
        public ArticleValidation()
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
