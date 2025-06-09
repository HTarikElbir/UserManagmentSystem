using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services.Validation;

public class DepartmentValidationService : IDepartmentValidationService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentValidationService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }
    public async Task ValidateByIdAsync(int id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        if (department == null)
            throw new Exception($"Department with ID {id} does not exist.");
    }

    public async Task ValidateByNameAsync(string name)
    {
       var department = await _departmentRepository.GetByNameAsync(name);
       
       if (department == null)
           throw new Exception($"Department with name {name} does not exist.");
    }
}