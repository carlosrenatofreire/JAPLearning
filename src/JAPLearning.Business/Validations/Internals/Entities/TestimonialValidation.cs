using FluentValidation;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Validations.Internals.Entities
{
    public class TestimonialValidation : AbstractValidator<Testimonial>
    {
        public TestimonialValidation()
        {
            RuleFor(t => t.AuthorName)
                .NotEmpty().WithMessage("O nome do autor é obrigatório.")
                .MaximumLength(150).WithMessage("O nome do autor deve ter no máximo 150 caracteres.");

            RuleFor(t => t.Role)
                .NotEmpty().WithMessage("O cargo/função do autor é obrigatório.")
                .MaximumLength(100).WithMessage("O cargo deve ter no máximo 100 caracteres.");

            RuleFor(t => t.Quote)
                .NotEmpty().WithMessage("O testemunho é obrigatório.")
                .MinimumLength(10).WithMessage("O testemunho deve ter no mínimo 10 caracteres.")
                .MaximumLength(1000).WithMessage("O testemunho deve ter no máximo 1000 caracteres.");

            RuleFor(t => t.Rating)
                .InclusiveBetween(1, 5).WithMessage("A avaliação deve estar entre 1 e 5.");

            RuleFor(t => t.LinkedinUrl)
                .MaximumLength(500).WithMessage("O URL do LinkedIn deve ter no máximo 500 caracteres.")
                .When(t => !string.IsNullOrEmpty(t.LinkedinUrl));
        }
    }
}
