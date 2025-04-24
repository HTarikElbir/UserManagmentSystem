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

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsers();
    }

    public Task<List<User>> GetUsersByDepartmentAsync(string departmentName)
    {
        return _userRepository.GetUsersByDepartmentAsync(departmentName);
    }

    public Task<List<User>> GetUsersByRoleAsync(string roleName)
    {
        return _userRepository.GetUsersByRoleAsync(roleName);
    }

    public Task<User?> GetUserByIdAsync(int userId)
    {
        return _userRepository.GetByIdUser(userId);
    }

    public Task<User?> GetUserByEmailAsync(string email)
    {
        return _userRepository.GetByEmail(email);
    }
}