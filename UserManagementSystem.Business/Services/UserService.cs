using AutoMapper;
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository; 
        _mapper = mapper;
    }
    
    public async Task AddUserAsync(User user)
    {
        await _userRepository.AddUserAsync(user);
    }
    
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        // Fetch all users from the repository
        var users = await _userRepository.GetAllUsers();
    
        // Map the users to UserDto objects, including their roles
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        // Return the list of mapped UserDto objects
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
        var userDto = _mapper.Map<UserDto>(user);

        // Return the mapped UserDto object
        return userDto;
    }

    // Updates the user entity with new information
    public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto)
    {
        // Retrieve the user by their ID
        var user = await _userRepository.GetByIdUser(userId);

        // If the user does not exist, return false
        if (user == null)
        {
            return false;
        }

        // Update user properties with the new values
        _mapper.Map(userUpdateDto, user);
    
        // Save the updated user entity
        await _userRepository.UpdateUserAsync(user);
    
        // Return true to indicate the update was successful
        return true;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdUser(userId);
        
        if (user == null)
        {
            return false;
        }
        
        await _userRepository.DeleteUserAsync(userId);
        return true;
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