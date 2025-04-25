using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository; 
    }
    
    public async Task AddUserAsync(User user)
    {
        await _userRepository.AddUserAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(int userId)
    {
        await _userRepository.DeleteUserAsync(userId);
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        // Fetch all users from the repository
        var users = await _userRepository.GetAllUsers();
    
        // Map the users to UserDto objects, including their roles
        var userDtos = users.Select(u => new UserDto()
        {
            UserId = u.UserId,
            Username = u.UserName, 
            Email = u.Email, 
            Department = u.Department, 
            Roles = u.UserRoles.Select(r => new RoleDto()
                {
                    RoleId = r.RoleId, 
                    RoleName = r.Role.RoleName 
                })
                .ToList() // Convert roles to a list
        }).ToList();
    
        // Return the list of UserDto objects
        return userDtos;
    }

    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        // Fetch the user by ID from the repository
        var user = await _userRepository.GetByIdUser(userId);

        // If the user is not found, return null
        if (user == null)
            return null;
    
        // Map the user to a UserDto object, including their roles
        var userDto = new UserDto() 
        {
            UserId = user.UserId, 
            Username = user.UserName, 
            Email = user.Email, 
            Department = user.Department, 
            Roles = user.UserRoles.Select(r => new RoleDto() 
                {
                    RoleId = r.RoleId, 
                    RoleName = r.Role.RoleName 
                })
                .ToList() // Convert roles to a list
        };

        // Return the mapped UserDto object
        return userDto;
    }

    public Task<List<User>> GetUsersByDepartmentAsync(string departmentName)
    {
        return _userRepository.GetUsersByDepartmentAsync(departmentName);
    }

    public Task<List<User>> GetUsersByRoleAsync(string roleName)
    {
        return _userRepository.GetUsersByRoleAsync(roleName);
    }

  

    public Task<User?> GetUserByEmailAsync(string email)
    {
        return _userRepository.GetByEmail(email);
    }
}