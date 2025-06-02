using UserManagementSystem.Business.Dtos.User;

namespace UserManagementSystem.Business.Interfaces.Validation;

public interface IUserValidationService
{
    Task ValidateUserExistAsync(int userId);
    Task ValidateUserExistByRoleAsync(string roleName, int page, int pageSize);
    Task ValidateUserExistByDepartmentAsync(int departmentId, int page, int pageSize);
    Task ValidateUserRoleNotExistAsync(int userId, int roleId);
    Task ValidateUpdateRequestAsync(int userId, UserUpdateDto userUpdateDto);
    Task ValidateAddRequestAsync(UserAddDto userAddDto);
}