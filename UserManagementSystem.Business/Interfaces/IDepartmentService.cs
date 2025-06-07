using UserManagementSystem.Business.Dtos.Department;
using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetAllAsync(int page, int pageSize);
    Task<DepartmentDto> GetByIdAsync(int id);
    Task<DepartmentDto> GetByNameAsync(string name);
    Task<bool> AddAsync(DepartmentAddDto departmentAddDto);
    Task<bool> RemoveAsync(int id);
    Task<bool> UpdateAsync(int id, DepartmentUpdateDto departmentUpdateDto);
}