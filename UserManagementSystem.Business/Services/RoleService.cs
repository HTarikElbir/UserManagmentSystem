using AutoMapper;
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Interfaces;
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
    public async Task<RoleDto?> GetUserByIdAsync(int roleId)
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

    // Adds a new role
    public Task<bool> AddRoleAsync(RoleAddDto roleAddDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateRoleAsync(int roleId, RoleUpdateDto roleUpdateDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteRoleAsync(int roleId)
    {
        throw new NotImplementedException();
    }



    public Task<RoleDto?> GetRoleByIdAsync(int roleId)
    {
        throw new NotImplementedException();
    }
}