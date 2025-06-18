using AutoMapper;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<UserService> _logger;
    
    // Initializes the dependencies of the UserService class.
    public UserService(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IMapper mapper, 
        IUserValidationService userValidator,
        IRoleValidationService roleValidator,
        IDepartmentValidationService departmentValidator,
        IPasswordHasher passwordHasher,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository; 
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
        _userValidator = userValidator;
        _roleValidator = roleValidator;
        _departmentValidator = departmentValidator;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }
   
    // Retrieves all users and maps them to a list of UserDto objects.
    public async Task<List<UserDto>> GetAllUsersAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Getting all users - Page: {Page}, PageSize: {PageSize}", page, pageSize);
            
            var users = await _userRepository.GetAllUsersAsync(page, pageSize);
            var userDtos = _mapper.Map<List<UserDto>>(users);
            
            _logger.LogInformation("Retrieved {Count} users", userDtos.Count);
            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all users - Page: {Page}, PageSize: {PageSize}", page, pageSize);
            throw;
        }
    }

    public async Task<List<UserDto>> GetAllUsersForReportAsync()
    {
        try
        {
            _logger.LogInformation("Getting all users for report");
            
            var users = await _userRepository.GetAllUsersWithoutPaginationAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            
            _logger.LogInformation("Retrieved {Count} users for report", userDtos.Count);
            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all users for report");
            throw;
        }
    }

    public async Task<List<UserDto>> GetUsersByDepartmentForReportAsync(int departmentId)
    {
        try
        {
            _logger.LogInformation("Getting users by department for report");
            
            await _departmentValidator.ValidateByIdAsync(departmentId);
        
            var users = await _userRepository.GetUsersByDepartmentWithoutPaginationAsync(departmentId);
        
            var userDtos = _mapper.Map<List<UserDto>>(users);
            
            _logger.LogInformation("Retrieved {Count} users for report", userDtos.Count);
        
            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get users by department for report");
            throw;
        }
        
    }

    public async Task<List<UserDto>> GetUsersByRoleForReportAsync(int roleId)
    {
        try
        {
            _logger.LogInformation("Getting users by role for report");
            
            await _roleValidator.ValidateRoleExistAsync(roleId);
        
            var users = await _userRepository.GetUsersByRoleWithoutPaginationAsync(roleId);
        
            var userDtos = _mapper.Map<List<UserDto>>(users);
            
            _logger.LogInformation("Retrieved {Count} users for report", userDtos.Count);
        
            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get users by role for report");
            throw;
        }
    }

    // Retrieves a user by their ID and maps it to a UserDto object.
    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        try
        {
            _logger.LogInformation("Getting user by ID: {UserId}", userId);
            
            await _userValidator.ValidateUserExistAsync(userId); 
            var user = await _userRepository.GetByIdUserAsync(userId);
            var userDto = _mapper.Map<UserDto>(user);
            
            _logger.LogInformation("User retrieved successfully: {UserId}", userId);
            return userDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user by ID: {UserId}", userId);
            throw;
        }
    }

    // Updates the user entity with new information
    public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto)
    {
        try
        {
            _logger.LogInformation("Updating user: {UserId}", userId);
            
            await _userValidator.ValidateUpdateRequestAsync(userId, userUpdateDto);
            
            var user = await _userRepository.GetByIdUserAsync(userId);
            _mapper.Map(userUpdateDto, user);
            
            await _userRepository.UpdateUserAsync(user!);
            
            _logger.LogInformation("User updated successfully: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user: {UserId}", userId);
            throw;
        }
    }

    // Deletes a user by their ID.
    public async Task<bool> DeleteUserAsync(int userId)
    {
        try
        {
            _logger.LogInformation("Deleting user: {UserId}", userId);
            
            await _userValidator.ValidateUserExistAsync(userId);
            await _userRepository.DeleteUserAsync(userId);
            
            _logger.LogInformation("User deleted successfully: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete user: {UserId}", userId);
            throw;
        }
    }

    // Retrieves users by department name, maps them to UserDto objects, and returns the list.
    public async Task<List<DepartmentUserDto>> GetUsersByDepartmentAsync(int departmentId, int page = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Getting users by department - DepartmentId: {DepartmentId}, Page: {Page}, PageSize: {PageSize}", departmentId, page, pageSize);
        
            await _userValidator.ValidateUserExistByDepartmentAsync(departmentId, page, pageSize);
        
            var users = await _userRepository.GetUsersByDepartmentAsync(departmentId, page, pageSize);
        
            var userDtos = _mapper.Map<List<DepartmentUserDto>>(users);
        
            _logger.LogInformation("Retrieved {Count} users for department {DepartmentId}", userDtos.Count, departmentId);
        
            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get users by department - DepartmentId: {DepartmentId}, Page: {Page}, PageSize: {PageSize}", departmentId, page, pageSize);
            throw;
        }
    }
    
    // Retrieves users by role name, maps them to UserDto objects, and returns the list.
    public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName, int page = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Getting users by role - RoleName: {RoleName}, Page: {Page}, PageSize: {PageSize}", roleName, page, pageSize);
        
            await _userValidator.ValidateUserExistByRoleAsync(roleName, page, pageSize);
        
            var users = await _userRepository.GetUsersByRoleAsync(roleName, page, pageSize);
        
            var userDtos = _mapper.Map<List<UserDto>>(users);
        
            _logger.LogInformation("Retrieved {Count} users for role {RoleName}", userDtos.Count, roleName);
        
            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get users by role - RoleName: {RoleName}, Page: {Page}, PageSize: {PageSize}", roleName, page, pageSize);
            throw;
        }
    }
    
    // Retrieves a user by their email address.
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            _logger.LogInformation("Getting user by email: {Email}", email);
        
            var user = await _userRepository.GetUserByEmailAsync(email);
        
            if (user != null)
            {
                _logger.LogInformation("User found by email: {Email}, UserId: {UserId}", email, user.UserId);
            }
            else
            {
                _logger.LogInformation("User not found by email: {Email}", email);
            }
        
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user by email: {Email}", email);
            throw;
        }
    }

    // Adds a new user to the system after validating the input and hashing the password.
    public async Task<bool> AddUserAsync(UserAddDto userAddDto)
    {
        try
        {
            _logger.LogInformation("Adding new user: {Email}", userAddDto.Email);
            
            await _userValidator.ValidateAddRequestAsync(userAddDto);
            
            var user = _mapper.Map<User>(userAddDto);
            user.Password = _passwordHasher.HashPassword(userAddDto.Password);
            
            await _userRepository.AddUserAsync(user);
            
            _logger.LogInformation("User added successfully: {Email}, UserId: {UserId}", userAddDto.Email, user.UserId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add user: {Email}", userAddDto.Email);
            throw;
        }
    }
    
    public async Task<bool> AssignRoleToUserAsync(AssignRoleDto assignRoleDto)
    {
        try
        {
            _logger.LogInformation("Assigning role {RoleId} to user {UserId}", assignRoleDto.RoleId, assignRoleDto.UserId);
            
            await _userValidator.ValidateUserExistAsync(assignRoleDto.UserId);
            await _roleValidator.ValidateRoleExistAsync(assignRoleDto.RoleId);
            await _userValidator.ValidateUserRoleNotExistAsync(assignRoleDto.UserId, assignRoleDto.RoleId);
            
            var userRole = _mapper.Map<UserRole>(assignRoleDto);
            await _userRoleRepository.AssignRoleToUserAsync(userRole);
            
            _logger.LogInformation("Role {RoleId} assigned to user {UserId} successfully", assignRoleDto.RoleId, assignRoleDto.UserId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assign role {RoleId} to user {UserId}", assignRoleDto.RoleId, assignRoleDto.UserId);
            throw;
        }
    }

    public async Task<bool> RemoveRoleFromUserAsync(RemoveRoleDto removeRoleDto)
    {
        try
        {
            _logger.LogInformation("Removing role {RoleId} from user {UserId}", removeRoleDto.RoleId, removeRoleDto.UserId);
            
            await _userValidator.ValidateUserExistAsync(removeRoleDto.UserId);
            await _roleValidator.ValidateRoleCanBeRemovedAsync(removeRoleDto.UserId, removeRoleDto.RoleId);
            
            var removedRole = _mapper.Map<UserRole>(removeRoleDto);
            await _userRoleRepository.RemoveRoleFromUserAsync(removedRole);
            
            _logger.LogInformation("Role {RoleId} removed from user {UserId} successfully", removeRoleDto.RoleId, removeRoleDto.UserId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove role {RoleId} from user {UserId}", removeRoleDto.RoleId, removeRoleDto.UserId);
            throw;
        }
    }
}