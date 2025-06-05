using UserManagementSystem.Business.Dtos.Department;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.Services;

public class DepartmentService: IDepartmentService
{
    public Task<IList<Department>> GetAllAsync()
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

    public Task AddAsync(DepartmentAddDto departmentAddDto)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(int departmentId)
    {
        throw new NotImplementedException();
    }
}