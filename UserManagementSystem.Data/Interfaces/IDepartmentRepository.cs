using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Data.Interfaces;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task<Department?> GetByNameAsync(string name);
    Task<Department> AddAsync(Department department);
    Task<Department> UpdateAsync(Department department);
    Task DeleteAsync(int id);
}