using FluentValidation;
using Microsoft.Extensions.Logging;
using UserManagementSystem.Business.Dtos.Role;
using UserManagementSystem.Business.Dtos.User;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services.Validation;

public class UserValidationService : IUserValidationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IValidator<UserUpdateDto> _updateValidator;
    private readonly IValidator<UserAddDto> _addValidator;
    private readonly ILogger<UserValidationService> _logger;
    
    public UserValidationService(IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IValidator<UserUpdateDto> updateValidator, 
        IValidator<UserAddDto> addValidator,
        ILogger<UserValidationService> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;       
        _userRoleRepository = userRoleRepository;
        _updateValidator = updateValidator;
        _addValidator = addValidator;
        _logger = logger;
    }
    
    // Validates if a user exists by their ID.
    public async Task ValidateUserExistAsync(int userId)
    {
        try
        {
            var user = await _userRepository.GetByIdUserAsync(userId);
            
            if (user == null)
            {
                _logger.LogWarning("User not found validation failed - UserId: {UserId}", userId);
                throw new Exception($"User with ID {userId} does not exist.");
            }
        }
        catch (Exception ex) when (ex.Message != $"User with ID {userId} does not exist.")
        {
            _logger.LogError(ex, "Failed to validate user existence - UserId: {UserId}", userId);
            throw;
        }
    }
    
    // Validates if a user exists by their Role.
    public async Task ValidateUserExistByRoleAsync(string roleName, int page = 1, int pageSize = 10)
    { 
        try
        {
            var users = await _userRepository.GetUsersByRoleAsync(roleName, page, pageSize);
            
            if (users == null || users.Count == 0)
            {
                _logger.LogWarning("No users found for role validation - RoleName: {RoleName}, Page: {Page}, PageSize: {PageSize}", 
                    roleName, page, pageSize);
                throw new Exception("No users found for the specified role.");
            }
        }
        catch (Exception ex) when (ex.Message != "No users found for the specified role.")
        {
            _logger.LogError(ex, "Failed to validate users by role - RoleName: {RoleName}, Page: {Page}, PageSize: {PageSize}", 
                roleName, page, pageSize);
            throw;
        }
    }
    
    // Validates if a user exists by their Department.
    public async Task ValidateUserExistByDepartmentAsync(int departmentId, int page = 1, int pageSize = 10)
    {
        try
        {
            var users = await _userRepository.GetUsersByDepartmentAsync(departmentId, page, pageSize);
            
            if (users == null || users.Count == 0)
            {
                _logger.LogWarning("No users found for department validation - DepartmentId: {DepartmentId}, Page: {Page}, PageSize: {PageSize}", 
                    departmentId, page, pageSize);
                throw new Exception("No users found for the specified department.");
            }
        }
        catch (Exception ex) when (ex.Message != "No users found for the specified department.")
        {
            _logger.LogError(ex, "Failed to validate users by department - DepartmentId: {DepartmentId}, Page: {Page}, PageSize: {PageSize}", 
                departmentId, page, pageSize);
            throw;
        }
    }

    public async Task ValidateUserRoleNotExistAsync(int userId, int roleId)
    {
        try
        {
            var userRoles = await _userRoleRepository.GetRolesByUserIdAsync(userId);
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            
            if (userRoles.Contains(role!.RoleName))
            {
                _logger.LogWarning("User already has role validation failed - UserId: {UserId}, RoleId: {RoleId}, RoleName: {RoleName}", 
                    userId, roleId, role.RoleName);
                throw new Exception("User already has this role assigned.");
            }
        }
        catch (Exception ex) when (ex.Message != "User already has this role assigned.")
        {
            _logger.LogError(ex, "Failed to validate user role not exist - UserId: {UserId}, RoleId: {RoleId}", userId, roleId);
            throw;
        }
    }
    
    // Validates for updating a user.
    public async Task ValidateUpdateRequestAsync(int userId, UserUpdateDto userUpdateDto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(userUpdateDto);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("User update validation failed - UserId: {UserId}, Email: {Email}, Errors: {Errors}", 
                    userId, userUpdateDto.Email, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(validationResult.Errors);
            }
            
            await ValidateUserExistAsync(userId);
        }
        catch (Exception ex) when (ex is not ValidationException)
        {
            _logger.LogError(ex, "Failed to validate user update request - UserId: {UserId}, Email: {Email}", 
                userId, userUpdateDto.Email);
            throw;
        }
    }

    // Validates for adding a new user.
    public async Task ValidateAddRequestAsync(UserAddDto userAddDto)
    {
        try
        {
            var validationResult = await _addValidator.ValidateAsync(userAddDto);
            
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("User add validation failed - Email: {Email}, Username: {Username}, Errors: {Errors}", 
                    userAddDto.Email, userAddDto.UserName, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(validationResult.Errors);
            }
            
            var existingUser = await _userRepository.GetUserByEmailAsync(userAddDto.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("User email already exists validation failed - Email: {Email}", userAddDto.Email);
                throw new Exception($"User with email {userAddDto.Email} already exists.");
            }
        }
        catch (Exception ex) when (ex is not ValidationException && 
                                   ex.Message != $"User with email {userAddDto.Email} already exists.")
        {
            _logger.LogError(ex, "Failed to validate user add request - Email: {Email}, Username: {Username}", 
                userAddDto.Email, userAddDto.UserName);
            throw;
        }
    }
    
    
}