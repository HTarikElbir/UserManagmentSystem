using FluentValidation;
using UserManagementSystem.Business.Dtos.Role;

namespace UserManagementSystem.Business.Validators;

public class RoleUpdateDtoValidator : AbstractValidator<RoleUpdateDto>
{
    public RoleUpdateDtoValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Role name is required and must be less than 50 characters.");
    }
}