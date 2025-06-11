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
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMapper _mapper;
    private readonly IUserValidationService _userValidator;
    private readonly IRoleValidationService _roleValidator;
    private readonly IDepartmentValidationService _departmentValidator;
    private readonly IPasswordHasher _passwordHasher;
    
    // Initializes the dependencies of the UserService class.
    public UserService(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IMapper mapper, 
        IUserValidationService userValidator,
        IRoleValidationService roleValidator,
        IDepartmentValidationService departmentValidator,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository; 
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
        _userValidator = userValidator;
        _roleValidator = roleValidator;
        _departmentValidator = departmentValidator;
        _passwordHasher = passwordHasher;
    }
   
    // Retrieves all users and maps them to a list of UserDto objects.
    public async Task<List<UserDto>> GetAllUsersAsync(int page = 1, int pageSize = 10)
    {
        var users = await _userRepository.GetAllUsersAsync(page, pageSize);
        
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        return userDtos;
    }

    public async Task<List<UserDto>> GetAllUsersForReportAsync()
    {
        var users = await _userRepository.GetAllUsersWithoutPaginationAsync();
        
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        return userDtos;
    }

    public async Task<List<UserDto>> GetUsersByDepartmentForReportAsync(int departmentId)
    {
        await _departmentValidator.ValidateByIdAsync(departmentId);
        
        var users = await _userRepository.GetUsersByDepartmentWithoutPaginationAsync(departmentId);
        
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        return userDtos;
    }

    public async Task<List<UserDto>> GetUsersByRoleForReportAsync(int roleId)
    {
        await _roleValidator.ValidateRoleExistAsync(roleId);
        
        var users = await _userRepository.GetUsersByRoleWithoutPaginationAsync(roleId);
        
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        return userDtos;
    }

    // Retrieves a user by their ID and maps it to a UserDto object.
    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        await _userValidator.ValidateUserExistAsync(userId); 
        
        var user = await _userRepository.GetByIdUserAsync(userId);
        
        var userDto = _mapper.Map<UserDto>(user);
        
        return userDto;
    }

    // Updates the user entity with new information
    public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto)
    {
        await _userValidator.ValidateUpdateRequestAsync(userId, userUpdateDto);
        
        var user = await _userRepository.GetByIdUserAsync(userId);
        
        _mapper.Map(userUpdateDto, user);
        
        await _userRepository.UpdateUserAsync(user!);
        
        return true;
    }

    // Deletes a user by their ID.
    public async Task<bool> DeleteUserAsync(int userId)
    {
        await _userValidator.ValidateUserExistAsync(userId);
        
        await _userRepository.DeleteUserAsync(userId);
        
        return true;
    }

    // Retrieves users by department name, maps them to UserDto objects, and returns the list.
    public async Task<List<DepartmentUserDto>> GetUsersByDepartmentAsync(int departmentId, int page = 1, int pageSize = 10)
    {
        await _userValidator.ValidateUserExistByDepartmentAsync(departmentId, page, pageSize);
        
        var users = await _userRepository.GetUsersByDepartmentAsync(departmentId, page, pageSize);;
        
        var userDtos = _mapper.Map<List<DepartmentUserDto>>(users);
        
        return userDtos;
    }
    
    // Retrieves users by role name, maps them to UserDto objects, and returns the list.
    public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName, int page = 1, int pageSize = 10)
    {
        await _userValidator.ValidateUserExistByRoleAsync(roleName, page, pageSize);
        
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
        await _userValidator.ValidateAddRequestAsync(userAddDto);
        
        var user = _mapper.Map<User>(userAddDto);
        
        user.Password = _passwordHasher.HashPassword(userAddDto.Password);
        
        await _userRepository.AddUserAsync(user);
        
        return true;
    }
    
    public async Task<bool> AssignRoleToUserAsync(AssignRoleDto assignRoleDto)
    {
        await _userValidator.ValidateUserExistAsync(assignRoleDto.UserId);
        
        await _roleValidator.ValidateRoleExistAsync(assignRoleDto.RoleId);
        
        await _userValidator.ValidateUserRoleNotExistAsync(assignRoleDto.UserId, assignRoleDto.RoleId);
        
        var userRole = _mapper.Map<UserRole>(assignRoleDto);
        
        await _userRoleRepository.AssignRoleToUserAsync(userRole);
        
        return true;
    }

    public async Task<bool> RemoveRoleFromUserAsync(RemoveRoleDto removeRoleDto)
    {
        await _userValidator.ValidateUserExistAsync(removeRoleDto.UserId);
        
        await _roleValidator.ValidateRoleCanBeRemovedAsync(removeRoleDto.UserId, removeRoleDto.RoleId);
        
        var removedRole = _mapper.Map<UserRole>(removeRoleDto);
        
        await _userRoleRepository.RemoveRoleFromUserAsync(removedRole);

        return true;
    }
}