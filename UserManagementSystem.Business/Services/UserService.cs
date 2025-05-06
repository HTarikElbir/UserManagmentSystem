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
    private readonly IPasswordHasher _passwordHasher;
    
    // Initializes the dependencies of the UserService class.
    public UserService(
        IUserRepository userRepository, 
        IMapper mapper, 
        IValidator<UserAddDto> validator, 
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository; 
        _mapper = mapper;
        _validator = validator;
        _passwordHasher = passwordHasher;
    }
   
    // Retrieves all users and maps them to a list of UserDto objects.
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        return userDtos;
    }
    
    // Retrieves a user by their ID and maps it to a UserDto object.
    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdUserAsync(userId);
        
        if (user == null)
            return null;
        
        var userDto = _mapper.Map<UserDto>(user);
        
        return userDto;
    }

    // Updates the user entity with new information
    public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto)
    {
        var user = await _userRepository.GetByIdUserAsync(userId);
        
        if (user == null)
        {
            return false;
        }
        
        _mapper.Map(userUpdateDto, user);
        
        await _userRepository.UpdateUserAsync(user);
        
        return true;
    }

    // Deletes a user by their ID.
    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdUserAsync(userId);
        
        if (user == null)
        {
            return false;
        }
        
        await _userRepository.DeleteUserAsync(userId);
        
        return true;
    }

    // Retrieves users by department name, maps them to UserDto objects, and returns the list.
    public async Task<List<UserDto>> GetUsersByDepartmentAsync(string departmentName)
    {
        var users = await _userRepository.GetUsersByDepartmentAsync(departmentName);
        
        if (users == null || users.Count == 0)
        {
            throw new Exception("No users found for the specified department.");
        }
        
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        return userDtos;
    }
    
    // Retrieves users by role name, maps them to UserDto objects, and returns the list.
    public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName)
    {
        var users = await _userRepository.GetUsersByRoleAsync(roleName);
        
        if (users == null || users.Count == 0)
        {
            throw new Exception("No users found for the specified role.");
        }
        
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        return userDtos;
    }
    
    // Retrieves a user by their email address.
    public Task<User?> GetUserByEmailAsync(string email)
    {
        return _userRepository.GetByEmailAsync(email);
    }
    
    // Adds a new user to the system after validating the input and hashing the password.
    public async Task<bool> AddUserAsync(UserAddDto userAddDto)
    {
        var validationResult = await _validator.ValidateAsync(userAddDto);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        // Check if a user with the same email already exists
        var existingUser = await _userRepository.GetByEmailAsync(userAddDto.Email);
        
        if (existingUser != null)
        {
            throw new Exception("A user with this email already exists.");
        }
        
        var hashPassword = _passwordHasher.HashPassword(userAddDto.Password);
        
        var user = _mapper.Map<User>(userAddDto);
        
        user.Password = hashPassword;
        
        await _userRepository.AddUserAsync(user);
        
        return true;
    }
}