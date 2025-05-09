using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services.Validation;

public class RoleValidationService : IRoleValidationService
{
    private readonly IRoleRepository _roleRepository;
    
    public RoleValidationService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
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
}