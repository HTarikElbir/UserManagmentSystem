using FluentValidation;
using UserManagementSystem.Business.Dtos.Department;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services.Validation;

public class DepartmentValidationService : IDepartmentValidationService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IValidator<DepartmentAddDto> _addValidator;
    private readonly IValidator<DepartmentUpdateDto> _updateValidator;

    public DepartmentValidationService(IDepartmentRepository departmentRepository,
        IValidator<DepartmentAddDto> addValidator,
        IValidator<DepartmentUpdateDto> updateValidator)
    {
        _departmentRepository = departmentRepository;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
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

    public async Task ValidateAddRequestAsync(DepartmentAddDto departmentAddDto)
    {
        var result = await _addValidator.ValidateAsync(departmentAddDto);
        
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }

    public async Task ValidateUpdateRequestAsync(DepartmentUpdateDto departmentUpdateDto)
    {
        var result = await _updateValidator.ValidateAsync(departmentUpdateDto);
        
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}