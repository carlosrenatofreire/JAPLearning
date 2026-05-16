using FluentValidation;
using MundoDev.Mvc.ViewModels.Parameters;

namespace MundoDev.Mvc.Validators.Parameters
{
    public class RoleValidator : AbstractValidator<RoleViewModel>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        }
    }
}
