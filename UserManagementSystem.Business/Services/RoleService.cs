using AutoMapper;
using FluentValidation;
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Dtos.Role;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

// Service class for role operations
public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<RoleAddDto> _addValidator;
    private readonly IRoleValidationService _roleValidationService;

    public RoleService(IRoleRepository roleRepository, 
        IMapper mapper, 
        IValidator<RoleAddDto> addValidator,
        IValidator<RoleUpdateDto> updateValidator,
        IRoleValidationService roleValidationService)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
        _addValidator = addValidator;
        _roleValidationService = roleValidationService;
    }
    
    // Retrieves all roles
    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllRolesAsync();
        
        var roleDtos = _mapper.Map<List<RoleDto>>(roles);
        
        return roleDtos;
    }
    
    // Retrieve the role by ID
    public async Task<RoleDto?> GetRoleByIdAsync(int roleId)
    {
        await _roleValidationService.ValidateRoleExistAsync(roleId);
        
        var role = await _roleRepository.GetRoleByIdAsync(roleId);
        
        var roleDto = _mapper.Map<RoleDto>(role);
        
        return roleDto;
    }
    
    // Retrieve the role by name
    public async Task<RoleDto?> GetRoleByNameAsync(string roleName)
    {
        
        var role = await _roleRepository.GetRoleByNameAsync(roleName);
        
        var roleDto = _mapper.Map<RoleDto>(role);
        
        return roleDto;
    }

    // Adds a new role
    public async Task<bool> AddRoleAsync(RoleAddDto roleAddDto)
    {
        // Add validation logic here if needed !!!
        var validationResult = await _addValidator.ValidateAsync(roleAddDto);
        
        if (!validationResult.IsValid)
        {
            throw new Exception("Validation failed");
        }
        
        var role = _mapper.Map<Role>(roleAddDto);
        
        await _roleRepository.AddRoleAsync(role);
        
        return true;
    }
    
    // Updates an existing role
    public async Task<bool> UpdateRoleAsync(int roleId, RoleUpdateDto roleUpdateDto)
    {
        await _roleValidationService.ValidateUpdateRequestAsync(roleId, roleUpdateDto);
        
        var role = await _roleRepository.GetRoleByIdAsync(roleId);
        
        _mapper.Map(roleUpdateDto, role);
        
        await _roleRepository.UpdateRoleAsync(role!);
        
        return true;
    }
    
    // Deletes a role by its ID
    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        await _roleValidationService.ValidateRoleExistAsync(roleId);
        
        await _roleRepository.DeleteRoleAsync(roleId);  
        
        return true;
    }
    
    
    
}