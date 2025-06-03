using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Data.Interfaces;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllAsync(int page, int pageSize);
    Task<Department?> GetByIdAsync(int id);
    Task<Department?> GetByNameAsync(string name);
    Task AddAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(int id);
}