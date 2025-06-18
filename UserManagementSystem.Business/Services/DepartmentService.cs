using AutoMapper;
using Microsoft.Extensions.Logging;
using UserManagementSystem.Business.Dtos.Department;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

public class DepartmentService: IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDepartmentValidationService _departmentValidationService;
    private readonly IMapper _mapper;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(IDepartmentRepository departmentRepository,
        IDepartmentValidationService departmentValidationService, 
        IMapper mapper,
        ILogger<DepartmentService> logger)
    {
        _departmentRepository = departmentRepository;
        _departmentValidationService = departmentValidationService;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<List<DepartmentDto>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Getting all departments - Page: {Page}, PageSize: {PageSize}", page, pageSize);
            
            var departments = await _departmentRepository.GetAllAsync(page, pageSize);
            var departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);

            _logger.LogInformation("Retrieved {Count} departments", departmentDtos.Count);
            return departmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all departments - Page: {Page}, PageSize: {PageSize}", page, pageSize);
            throw;
        }
    }

    public async Task<DepartmentDto> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Getting department by ID: {DepartmentId}", id);
            
            await _departmentValidationService.ValidateByIdAsync(id);
            
            var department = await _departmentRepository.GetByIdAsync(id);
            var departmentDto = _mapper.Map<DepartmentDto>(department);
            
            _logger.LogInformation("Department retrieved successfully: {DepartmentId}, Name: {DepartmentName}", id, departmentDto.DepartmentName);
            return departmentDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get department by ID: {DepartmentId}", id);
            throw;
        }
    }

    public async Task<DepartmentDto> GetByNameAsync(string name)
    {
        try
        {
            _logger.LogInformation("Getting department by name: {DepartmentName}", name);
            
            await _departmentValidationService.ValidateByNameAsync(name);
            
            var department = await _departmentRepository.GetByNameAsync(name);
            var departmentDto = _mapper.Map<DepartmentDto>(department);
            
            _logger.LogInformation("Department retrieved successfully: {DepartmentName}, DepartmentId: {DepartmentId}", name, departmentDto.DepartmentName);
            return departmentDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get department by name: {DepartmentName}", name);
            throw;
        }
    }

    public async Task<bool> AddAsync(DepartmentAddDto departmentAddDto)
    {
        try
        {
            _logger.LogInformation("Adding new department: {DepartmentName}", departmentAddDto.DepartmentName);
            
            await _departmentValidationService.ValidateAddRequestAsync(departmentAddDto);
            
            var department = _mapper.Map<Department>(departmentAddDto);
            await _departmentRepository.AddAsync(department);
            
            _logger.LogInformation("Department added successfully: {DepartmentName}, DepartmentId: {DepartmentId}", departmentAddDto.DepartmentName, department.DepartmentId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add department: {DepartmentName}", departmentAddDto.DepartmentName);
            throw;
        }
    }
    
    public async Task<bool> UpdateAsync(int id, DepartmentUpdateDto departmentUpdateDto)
    {
        try
        {
            _logger.LogInformation("Updating department: {DepartmentId}", id);
            
            await _departmentValidationService.ValidateByIdAsync(id);
            await _departmentValidationService.ValidateUpdateRequestAsync(departmentUpdateDto);

            var department = await _departmentRepository.GetByIdAsync(id);
            _mapper.Map(departmentUpdateDto, department);
            
            await _departmentRepository.UpdateAsync(department!);
            
            _logger.LogInformation("Department updated successfully: {DepartmentId}, NewName: {DepartmentName}", id, departmentUpdateDto.DepartmentName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update department: {DepartmentId}", id);
            throw;
        }
    }
    
    public async  Task<bool> RemoveAsync(int departmentId)
    {
        try
        {
            _logger.LogInformation("Removing department: {DepartmentId}", departmentId);
            
            await _departmentValidationService.ValidateByIdAsync(departmentId);
            await _departmentRepository.RemoveAsync(departmentId);
            
            _logger.LogInformation("Department removed successfully: {DepartmentId}", departmentId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove department: {DepartmentId}", departmentId);
            throw;
        }
    }
}