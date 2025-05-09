using FluentValidation;
using UserManagementSystem.Business.Dtos.Role;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services.Validation;

public class RoleValidationService : IRoleValidationService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IValidator<RoleUpdateDto> _updateValidator;  
    
    public RoleValidationService(IRoleRepository roleRepository, IValidator<RoleUpdateDto> updateValidator )
    {
        _roleRepository = roleRepository;
        _updateValidator = updateValidator;
    }
    // Validates if a role exists by its ID
    public async Task ValidateRoleExistAsync(int roleId)
    { 
        var role = await _roleRepository.GetRoleByIdAsync(roleId);
        
        if (role == null)
        {
            throw new Exception($"Role with ID {roleId} does not exist.");
        }
    }
    
    // Validates the update request
    public async Task ValidateUpdateRequestAsync(int roleId, RoleUpdateDto roleUpdateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(roleUpdateDto);

        if (!validationResult.IsValid)
        {
            throw new Exception("Validation failed");
        }

        await ValidateRoleExistAsync(roleId);
    }
}