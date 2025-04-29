using AutoMapper;
using FluentValidation;
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
    private readonly IValidator<UserAddDto> _validator;
    public UserService(IUserRepository userRepository, IMapper mapper, IValidator<UserAddDto> validator)
    {
        _userRepository = userRepository; 
        _mapper = mapper;
        _validator = validator;
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
        // Retrieve the user by their ID from the repository
        var user = await _userRepository.GetByIdUser(userId);

        // Check if the user exists
        if (user == null)
        {
            // Return false if the user was not found
            return false;
        }

        // Delete the user through the repository
        await _userRepository.DeleteUserAsync(userId);
        
        // Return true indicating successful deletion
        return true;
    }

   
    public async Task<List<UserDto>> GetUsersByDepartmentAsync(string departmentName)
    {
        // Retrieve users from the data layer
        var users = await _userRepository.GetUsersByDepartmentAsync(departmentName);

        // Check if any users were found
        if (users == null || users.Count == 0)
        {
            // Throw an exception if no users are returned
            throw new Exception("No users found for the specified department.");
        }
        
        // Map the list of User entities to a list of UserDto objects
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        // Return the mapped DTOs
        return userDtos;
    }

    public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName)
    {
        // Fetch users by role from repository
        var users = await _userRepository.GetUsersByRoleAsync(roleName);

        // Check if no users found for the specified role
        if (users == null || users.Count == 0)
        {
            throw new Exception("No users found for the specified role.");
        }

        // Map users to UserDto
        var userDtos = _mapper.Map<List<UserDto>>(users);
    
        // Return the list of UserDto
        return userDtos;
    }
    
    public Task<User?> GetUserByEmailAsync(string email)
    {
        return _userRepository.GetByEmail(email);
    }
    
     
    public async Task AddUserAsync(UserAddDto userAddDto)
    {
        // Validate the incoming DTO using FluentValidation
        var validationResult = await _validator.ValidateAsync(userAddDto);
        
        if (!validationResult.IsValid)
        {
            // Handle validation errors
            throw new ValidationException(validationResult.Errors);
        }
        
        // Map the DTO to the User entity
        var user = _mapper.Map<User>(userAddDto);
        
        // Save the new user to the repository
        await _userRepository.AddUserAsync(user);
    }
}