using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Data.Interfaces
{
    // Interface for user-related data access operations.
    // Defines methods for managing users in the system.
    public interface IUserRepository
    {
        // Returns a list of all users.
        Task<List<User>> GetAllUsers();
        
        // Returns users that belong to a specific department.
        Task<List<User>> GetUsersByDepartmentAsync(string department);

        // Returns users that have a specific role.
        Task<List<User>> GetUsersByRoleAsync(string roleName);

        // Returns a user by their unique ID.
        Task<User?> GetByIdUser(int userId);
        
        // Returns a user by their Email
        Task<User?> GetByEmail(string email);
        
        // Adds a new user to the system.
        Task AddUserAsync(User user);

        // Updates an existing user.
        Task UpdateUserAsync(User user);

        // Deletes a user by their ID.
        Task DeleteUserAsync(int userId);
    }
}