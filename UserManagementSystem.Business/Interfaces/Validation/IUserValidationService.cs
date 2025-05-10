using UserManagementSystem.Business.Dtos.User;

namespace UserManagementSystem.Business.Interfaces.Validation;

public interface IUserValidationService
{
    Task ValidateUserExistAsync(int userId);
    Task ValidateUserExistByRoleAsync(string roleName);
    Task ValidateUserExistByDepartmentAsync(string departmentName);
    Task ValidateUpdateRequestAsync(int userId, UserUpdateDto userUpdateDto);
    Task ValidateAddRequestAsync(UserAddDto userAddDto);
}