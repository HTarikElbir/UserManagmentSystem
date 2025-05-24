using AutoMapper;
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUserValidationService _validator;
    private readonly IPasswordHasher _passwordHasher;
    
    // Initializes the dependencies of the UserService class.
    public UserService(
        IUserRepository userRepository, 
        IMapper mapper, 
        IUserValidationService validator,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository; 
        _mapper = mapper;
        _validator = validator;
        _passwordHasher = passwordHasher;
    }
   
    // Retrieves all users and maps them to a list of UserDto objects.
    public async Task<List<UserDto>> GetAllUsersAsync(int page = 1, int pageSize = 10)
    {
        var users = await _userRepository.GetAllUsersAsync(page, pageSize);
        
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        return userDtos;
    }
    
    // Retrieves a user by their ID and maps it to a UserDto object.
    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        await _validator.ValidateUserExistAsync(userId); 
        
        var user = await _userRepository.GetByIdUserAsync(userId);
        
        var userDto = _mapper.Map<UserDto>(user);
        
        return userDto;
    }

    // Updates the user entity with new information
    public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto)
    {
        await _validator.ValidateUserExistAsync(userId);
        
        var user = await _userRepository.GetByIdUserAsync(userId);
        
        _mapper.Map(userUpdateDto, user);
        
        await _userRepository.UpdateUserAsync(user!);
        
        return true;
    }

    // Deletes a user by their ID.
    public async Task<bool> DeleteUserAsync(int userId)
    {
        await _validator.ValidateUserExistAsync(userId);
        
        await _userRepository.DeleteUserAsync(userId);
        
        return true;
    }

    // Retrieves users by department name, maps them to UserDto objects, and returns the list.
    public async Task<List<UserDto>> GetUsersByDepartmentAsync(string departmentName, int page = 1, int pageSize = 10)
    {
        await _validator.ValidateUserExistByDepartmentAsync(departmentName, page, pageSize);
        
        var users = await _userRepository.GetUsersByDepartmentAsync(departmentName, page, pageSize);;
        
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        return userDtos;
    }
    
    // Retrieves users by role name, maps them to UserDto objects, and returns the list.
    public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName, int page = 1, int pageSize = 10)
    {
        await _validator.ValidateUserExistByRoleAsync(roleName, page, pageSize);
        
        var users = await _userRepository.GetUsersByRoleAsync(roleName, page, pageSize);
        
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        return userDtos;
    }
    
    // Retrieves a user by their email address.
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetUserByEmailAsync(email);
    }
    
    // Adds a new user to the system after validating the input and hashing the password.
    public async Task<bool> AddUserAsync(UserAddDto userAddDto)
    {
        await _validator.ValidateAddRequestAsync(userAddDto);
        
        var user = _mapper.Map<User>(userAddDto);
        
        user.Password = _passwordHasher.HashPassword(userAddDto.Password);
        
        await _userRepository.AddUserAsync(user);
        
        return true;
    }
}