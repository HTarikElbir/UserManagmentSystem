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
        
        var roleDtos = _mapper.Map<List<RoleDto>>(roles);
        
        return roleDtos;
    }
    
    // Retrieve the role by ID
    public async Task<RoleDto?> GetRoleByIdAsync(int roleId)
    {
        var role = await _roleRepository.GetRoleByIdAsync(roleId);
        
        if (role == null)
        {
            return null;
        }
        
        var roleDto = _mapper.Map<RoleDto>(role);
        
        return roleDto;
    }
    
    // Retrieve the role by name
    public async Task<RoleDto?> GetRoleByNameAsync(string roleName)
    {
        var role = await _roleRepository.GetRoleByNameAsync(roleName);
        
        if (role == null)
        {
            return null;
        }
        
        var roleDto = _mapper.Map<RoleDto>(role);
        
        return roleDto;
    }

    // Adds a new role
    public async Task<bool> AddRoleAsync(RoleAddDto roleAddDto)
    {
        // Add validation logic here if needed !!!
        
        var existingRole = await _roleRepository.GetRoleByNameAsync(roleAddDto.RoleName);
        
        if (existingRole != null)
        {
            throw new Exception("Role already exists.");
        }
        
        var role = _mapper.Map<Role>(roleAddDto);
        
        await _roleRepository.AddRoleAsync(role);
        
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