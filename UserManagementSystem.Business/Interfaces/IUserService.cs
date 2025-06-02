using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Data.Entities;

namespace UserManagementSystem.Business.Interfaces;

// Interface for user-related business logic operations.
public interface IUserService
{
    // Adds a new user
    Task<bool> AddUserAsync(UserAddDto userAddDto);
    
    // Updates an existing user
    Task<bool> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto);
    
    // Deletes a user by their ID
    Task<bool> DeleteUserAsync(int userId);
    
    // Get all users
    Task<List<UserDto>> GetAllUsersAsync(int page, int pageSize);
    
    // Get users by Department 
    Task<List<DepartmentUserDto>> GetUsersByDepartmentAsync(int departmentId, int page, int pageSize);
    
    // Get users by Role
    Task<List<UserDto>> GetUsersByRoleAsync(string roleName, int page, int pageSize);
    
    // Get user by ID
    Task<UserDto?> GetUserByIdAsync(int userId);
    
    // Get user by email
    Task<User?> GetUserByEmailAsync(string email);
    
    // Assign a role to a user
    Task<bool> AssignRoleToUserAsync(AssignRoleDto assignRoleDto);
    
    // Remove a role from a user
    Task<bool> RemoveRoleFromUserAsync(RemoveRoleDto removeRoleDto);
}