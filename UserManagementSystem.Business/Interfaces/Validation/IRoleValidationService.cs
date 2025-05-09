
namespace UserManagementSystem.Business.Interfaces.Validation;


// Interface for role validation service
public interface IRoleValidationService
{
    Task ValidateRoleExistAsync(int roleId);
}