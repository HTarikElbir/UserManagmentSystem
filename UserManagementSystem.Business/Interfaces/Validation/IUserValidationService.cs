using UserManagementSystem.Business.Dtos.User;

namespace UserManagementSystem.Business.Interfaces.Validation;

public interface IUserValidationService
{
    Task ValidateUserExistAsync(int userId);
    Task ValidateUserExistByRoleAsync(string roleName, int page, int pageSize);
    Task ValidateUserExistByDepartmentAsync(string departmentName, int page, int pageSize);
    Task ValidateUpdateRequestAsync(int userId, UserUpdateDto userUpdateDto);
    Task ValidateAddRequestAsync(UserAddDto userAddDto);
}