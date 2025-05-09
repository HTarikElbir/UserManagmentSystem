
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Dtos.Role;

namespace UserManagementSystem.Business.Interfaces.Validation;


// Interface for role validation service
public interface IRoleValidationService
{
    Task ValidateRoleExistAsync(int roleId);
    Task ValidateUpdateRequestAsync(int roleId, RoleUpdateDto roleUpdateDto);
    
    Task ValidateAddRequestAsync(RoleAddDto roleAddDto);
}