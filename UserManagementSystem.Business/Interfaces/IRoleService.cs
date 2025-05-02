using UserManagementSystem.Business.Dtos;

namespace UserManagementSystem.Business.Interfaces;

public interface IRoleService
{
    // Adds a new role
    Task<bool> AddRoleAsync(RoleAddDto roleAddDto);
    
    // Updates an existing user
    Task<bool> UpdateRoleAsync(int roleId, RoleUpdateDto roleUpdateDto);
    
    // Deletes a user by their ID
    Task<bool> DeleteRoleAsync(int roleId);
    
    // Get all users
    Task<List<RoleDto>> GetAllRolesAsync();
    
    // Get user by ID
    Task<RoleDto?> GetRoleByIdAsync(int roleId);
    
    Task<RoleDto?> GetRoleByNameAsync(string roleName);
    
}