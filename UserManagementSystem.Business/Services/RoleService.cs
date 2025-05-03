using AutoMapper;
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
    
    // Retrieves all roles
    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllRolesAsync();
        
        // Map the roles to RoleDto objects
        var roleDtos = _mapper.Map<List<RoleDto>>(roles);
        
        // Return the list of mapped RoleDto objects
        return roleDtos;
    }
    
    // Retrieve the role by ID
    public async Task<RoleDto?> GetRoleByIdAsync(int roleId)
    {
        
        var role = await _roleRepository.GetRoleByIdAsync(roleId);
        
        // Check if the role is null
        if (role == null)
        {
            return null;
        }
        
        // Map the role to RoleDto object
        var roleDto = _mapper.Map<RoleDto>(role);
        
        // Return the mapped RoleDto object
        return roleDto;
    }
    
    // Retrieve the role by name
    public async Task<RoleDto?> GetRoleByNameAsync(string roleName)
    {
        var role = await _roleRepository.GetRoleByNameAsync(roleName);
        
        // Check if the role is null
        if (role == null)
        {
            return null;
        }
        
        // Map the role to RoleDto object
        var roleDto = _mapper.Map<RoleDto>(role);
        
        // Return the mapped RoleDto object
        return roleDto;
    }

    // Adds a new role
    public async Task<bool> AddRoleAsync(RoleAddDto roleAddDto)
    {
        // Add validation logic here if needed !!!
        
        // Retrieve the role by name from the repository
        var existingRole = await _roleRepository.GetRoleByNameAsync(roleAddDto.RoleName);
        
        // Check if the role already exists
        if (existingRole != null)
        {
            throw new Exception("Role already exists.");
        }
        
        // Map the RoleAddDto to Role entity
        var role = _mapper.Map<Role>(roleAddDto);
        
        // Add the role to the repository
        await _roleRepository.AddRoleAsync(role);
        
        // Check if the role was added successfully
        return true;
    }

    public Task<bool> UpdateRoleAsync(int roleId, RoleUpdateDto roleUpdateDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        throw new NotImplementedException();
    }
    
}