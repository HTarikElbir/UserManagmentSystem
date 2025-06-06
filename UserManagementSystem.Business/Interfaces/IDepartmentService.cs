using UserManagementSystem.Business.Dtos.Department;
using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetAllAsync(int page, int pageSize);
    Task<Department?> GetByIdAsync(int id);
    Task<Department?> GetByNameAsync(string name);
    Task AddAsync(DepartmentAddDto departmentAddDto);
    Task RemoveAsync(int departmentId);
}