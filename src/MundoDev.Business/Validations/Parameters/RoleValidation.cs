using FluentValidation;
using MundoDev.Business.Models.Domains.Parameters;

namespace MundoDev.Business.Validations.Parameters
{
    public class RoleValidation : AbstractValidator<Role>
    {
        public RoleValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        }
    }
}
