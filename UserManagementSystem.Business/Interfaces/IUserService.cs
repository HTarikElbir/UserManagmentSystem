using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.Interfaces;

// Interface for user-related business logic operations.
public interface IUserService
{
    // Adds a new user
    Task AddUserAsync(User user);
    
    // Updates an existing user
    Task<bool> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto);
    
    // Deletes a user by their ID
    Task<bool> DeleteUserAsync(int userId);
    
    // Get all users
    Task<List<UserDto>> GetAllUsersAsync();
    
    // Get users by Department 
    Task<List<UserDto>> GetUsersByDepartmentAsync(string departmentName);
    
    // Get users by Role
    Task<List<User>> GetUsersByRoleAsync(string roleName);
    
    // Get user by ID
    Task<UserDto?> GetUserByIdAsync(int userId);
    
    // Get user by email
    Task<User?> GetUserByEmailAsync(string email);
}