using FluentValidation;
using Microsoft.Extensions.Logging;
using UserManagementSystem.Business.Dtos.Department;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services.Validation;

public class DepartmentValidationService : IDepartmentValidationService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IValidator<DepartmentAddDto> _addValidator;
    private readonly IValidator<DepartmentUpdateDto> _updateValidator;
    private readonly ILogger<DepartmentValidationService> _logger;

    public DepartmentValidationService(IDepartmentRepository departmentRepository,
        IValidator<DepartmentAddDto> addValidator,
        IValidator<DepartmentUpdateDto> updateValidator,
        ILogger<DepartmentValidationService> logger)
    {
        _departmentRepository = departmentRepository;
        _addValidator = addValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }
    public async Task ValidateByIdAsync(int id)
    {
        try
        {
            var department = await _departmentRepository.GetByIdAsync(id);

            if (department == null)
            {
                _logger.LogWarning("Department not found validation failed - DepartmentId: {DepartmentId}", id);
                throw new Exception($"Department with ID {id} does not exist.");
            }
        }
        catch (Exception ex) when (ex.Message != $"Department with ID {id} does not exist.")
        {
            _logger.LogError(ex, "Failed to validate department by ID: {DepartmentId}", id);
            throw;
        }
    }

    public async Task ValidateByNameAsync(string name)
    {
        try
        {
            var department = await _departmentRepository.GetByNameAsync(name);
           
            if (department == null)
            {
                _logger.LogWarning("Department not found validation failed - DepartmentName: {DepartmentName}", name);
                throw new Exception($"Department with name {name} does not exist.");
            }
        }
        catch (Exception ex) when (ex.Message != $"Department with name {name} does not exist.")
        {
            _logger.LogError(ex, "Failed to validate department by name: {DepartmentName}", name);
            throw;
        }
    }

    public async Task ValidateAddRequestAsync(DepartmentAddDto departmentAddDto)
    {
        try
        {
            var result = await _addValidator.ValidateAsync(departmentAddDto);
            
            if (!result.IsValid)
            {
                _logger.LogWarning("Department add validation failed - DepartmentName: {DepartmentName}, Errors: {Errors}", 
                    departmentAddDto.DepartmentName, string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(result.Errors);
            }

            var existingDepartment = await _departmentRepository.GetByNameAsync(departmentAddDto.DepartmentName);
            if (existingDepartment != null)
            {
                _logger.LogWarning("Department name already exists validation failed - DepartmentName: {DepartmentName}", 
                    departmentAddDto.DepartmentName);
                throw new Exception($"Department with name '{departmentAddDto.DepartmentName}' already exists.");
            }
        }
        catch (Exception ex) when (ex is not ValidationException && 
                                   ex.Message != $"Department with name '{departmentAddDto.DepartmentName}' already exists.")
        {
            _logger.LogError(ex, "Failed to validate department add request - DepartmentName: {DepartmentName}", 
                departmentAddDto.DepartmentName);
            throw;
        }
    }

    public async Task ValidateUpdateRequestAsync(DepartmentUpdateDto departmentUpdateDto)
    {
       
        try
        {
            var result = await _updateValidator.ValidateAsync(departmentUpdateDto);
            
            if (!result.IsValid)
            {
                _logger.LogWarning("Department update validation failed - DepartmentName: {DepartmentName}, Errors: {Errors}", 
                    departmentUpdateDto.DepartmentName, string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(result.Errors);
            }

            // Department name uniqueness check (excluding current department)
            var existingDepartment = await _departmentRepository.GetByNameAsync(departmentUpdateDto.DepartmentName);
            if (existingDepartment != null)
            {
                _logger.LogWarning("Department name already exists validation failed - DepartmentName: {DepartmentName}", 
                    departmentUpdateDto.DepartmentName);
                throw new Exception($"Department with name '{departmentUpdateDto.DepartmentName}' already exists.");
            }
        }
        catch (Exception ex) when (ex is not ValidationException && 
                                   ex.Message != $"Department with name '{departmentUpdateDto.DepartmentName}' already exists.")
        {
            _logger.LogError(ex, "Failed to validate department update request - DepartmentName: {DepartmentName}", 
                departmentUpdateDto.DepartmentName);
            throw;
        }
    }
}