using FluentValidation;
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Validators;

public class RoleAddDtoValidator : AbstractValidator<RoleDto>
{
    private readonly IRoleRepository _roleRepository;

    public RoleAddDtoValidator(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

        RuleFor(x => x.RoleName)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (roleName, cancellationToken) =>
            {
                var existingRole = await _roleRepository.GetRoleByNameAsync(roleName);
                return existingRole == null;
            })
            .WithMessage("Role name must be unique.");
        
    }
}