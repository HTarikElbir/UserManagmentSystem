using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Data.Interfaces
{
    // Interface for managing user-role relationships.
    // Defines methods for assigning, removing, and querying roles and users.
    public interface IUserRoleRepository
    {
        // Assigns a role to a user.
        Task AssignRoleToUserAsync(UserRole userRole);

        // Removes a specific role from a user.
        Task RemoveRoleFromUserAsync(int userId, int roleId);

        // Gets all roles assigned to a specific user.
        Task<List<Role>> GetRolesByUserIdAsync(int userId);

        // Gets all users who have a specific role.
        Task<List<User>> GetUsersByRoleIdAsync(int roleId);
    }
}