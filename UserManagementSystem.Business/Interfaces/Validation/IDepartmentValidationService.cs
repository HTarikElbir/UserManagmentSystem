using UserManagementSystem.Business.Dtos.Department;

namespace UserManagementSystem.Business.Interfaces.Validation;

public interface IDepartmentValidationService
{
    Task ValidateByIdAsync(int id);
    Task ValidateByNameAsync(string name);
    Task ValidateAddRequestAsync(DepartmentAddDto departmentAddDto);
    Task ValidateUpdateRequestAsync(DepartmentUpdateDto departmentUpdateDto);
}