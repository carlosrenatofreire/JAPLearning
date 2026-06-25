using FluentValidation;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Validations.Internals.Entities
{
    public class ArticleValidation : AbstractValidator<Article>
    {
        public ArticleValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome do artigo é obrigatório.")
                .MaximumLength(200).WithMessage("O nome do artigo deve ter no máximo 200 caracteres.");

            RuleFor(x => x.SubjectId)
                .NotEmpty().WithMessage("O assunto do artigo é obrigatório.");

            RuleFor(x => x.Slug)
                .MaximumLength(250).WithMessage("O slug deve ter no máximo 250 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Slug));
        }
    }
}
