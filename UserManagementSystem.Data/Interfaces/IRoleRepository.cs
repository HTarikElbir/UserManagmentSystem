using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Data.Interfaces
{
    // Interface for role-related data access operations.
    // Provides basic CRUD methods for managing roles in the system.
    public interface IRoleRepository
    {
        // Returns a list of all roles in the system.
        Task<List<Role>> GetAllRolesAsync(int page, int pageSize);

        // Returns a role by its unique identifier.
        Task<Role?> GetRoleByIdAsync(int roleId);

        // Returns a role by its unique name.
        Task<Role?> GetRoleByNameAsync(string roleName);

        // Adds a new role to the system.
        Task AddRoleAsync(Role role);

        // Updates an existing role.
        Task UpdateRoleAsync(Role role);

        // Deletes a role by its ID.
        Task DeleteRoleAsync(int roleId);
    }
}