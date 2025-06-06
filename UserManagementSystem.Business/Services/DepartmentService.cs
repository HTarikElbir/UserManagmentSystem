using AutoMapper;
using UserManagementSystem.Business.Dtos.Department;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

public class DepartmentService: IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;

    public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }
    public async Task<List<DepartmentDto>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        var departments = await _departmentRepository.GetAllAsync(page, pageSize);
        
        var departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);

        return departmentDtos;
    }

    public async Task<DepartmentDto> GetByIdAsync(int id)
    {
        // TODO: Add Validation Check here
        
        var department = await  _departmentRepository.GetByIdAsync(id);
        
        var departmentDto = _mapper.Map<DepartmentDto>(department);
        
        return departmentDto;
    }

    public async Task<DepartmentDto> GetByNameAsync(string name)
    {
        // TODO: Add Validation Check here 
        var department = await  _departmentRepository.GetByNameAsync(name);
        
        var departmentDto = _mapper.Map<DepartmentDto>(department);
        
        return departmentDto;
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