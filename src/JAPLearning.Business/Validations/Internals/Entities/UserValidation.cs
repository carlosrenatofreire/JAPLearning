using FluentValidation;
using JAPLearning.Business.Models.Domains.Entities;

namespace JAPLearning.Business.Validations.Internals.Entities
{
    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("O primeiro nome é obrigatório.")
                .MaximumLength(100).WithMessage("O primeiro nome deve ter no máximo 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("O apelido é obrigatório.")
                .MaximumLength(100).WithMessage("O apelido deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("Formato de email inválido.")
                .MaximumLength(200).WithMessage("O email deve ter no máximo 200 caracteres.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("O perfil do utilizador é obrigatório.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A palavra-passe é obrigatória.")
                .MinimumLength(6).WithMessage("A palavra-passe deve ter no mínimo 6 caracteres.");
        }
    }
}
