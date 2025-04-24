using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.Interfaces;

// Interface for user-related business logic operations.
public interface IUserService
{
    // Adds a new user
    Task AddUserAsync(User user);
    
    // Updates an existing user
    Task UpdateUserAsync(User user);
    
    // Deletes a user by their ID
    Task DeleteUserAsync(int userId);
    
    // Get all users
    Task<List<User>> GetAllUsersAsync();
    
    // Get users by Department 
    Task<List<User>> GetUsersByDepartmentAsync(string departmentName);
    
    // Get users by Role
    Task<List<User>> GetUsersByRoleAsync(string roleName);
    
    // Get user by ID
    Task<User?> GetUserByIdAsync(int userId);
    
    // Get user by email
    Task<User?> GetUserByEmailAsync(string email);
}