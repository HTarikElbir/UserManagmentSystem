using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Department?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Department?> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<Department> AddAsync(Department department)
    {
        throw new NotImplementedException();
    }

    public Task<Department> UpdateAsync(Department department)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}