using FluentValidation;
using Microsoft.Extensions.Logging;
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Dtos.Role;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services.Validation;

public class RoleValidationService : IRoleValidationService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;   
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IValidator<RoleUpdateDto> _updateValidator;  
    private readonly IValidator<RoleAddDto> _addValidator;
    private readonly ILogger<RoleValidationService> _logger;
    
    
    public RoleValidationService(IRoleRepository roleRepository, 
        IValidator<RoleUpdateDto> updateValidator, 
        IValidator<RoleAddDto> addValidator,
        IUserRoleRepository userRoleRepository,
        IUserRepository userRepository,
        ILogger<RoleValidationService> logger)
    {
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;    
        _userRepository = userRepository;   
        _updateValidator = updateValidator;
        _addValidator = addValidator;
        _logger = logger;
    }
    // Validates if a role exists by its ID
    public async Task ValidateRoleExistAsync(int roleId)
    { 
        try
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            
            if (role == null)
            {
                _logger.LogWarning("Role not found validation failed - RoleId: {RoleId}", roleId);
                throw new Exception($"Role with ID {roleId} does not exist.");
            }
        }
        catch (Exception ex) when (ex.Message != $"Role with ID {roleId} does not exist.")
        {
            _logger.LogError(ex, "Failed to validate role existence - RoleId: {RoleId}", roleId);
            throw;
        }
    }
    
    // Validates the update request
    public async Task ValidateUpdateRequestAsync(int roleId, RoleUpdateDto roleUpdateDto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(roleUpdateDto);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Role update validation failed - RoleId: {RoleId}, RoleName: {RoleName}, Errors: {Errors}", 
                    roleId, roleUpdateDto.RoleName, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(validationResult.Errors);
            }

            await ValidateRoleExistAsync(roleId);
        }
        catch (Exception ex) when (ex is not ValidationException)
        {
            _logger.LogError(ex, "Failed to validate role update request - RoleId: {RoleId}, RoleName: {RoleName}", 
                roleId, roleUpdateDto.RoleName);
            throw;
        }
    }

    public async Task ValidateAddRequestAsync(RoleAddDto roleAddDto)
    {
        try
        {
            var validationResult = await _addValidator.ValidateAsync(roleAddDto);

            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Role add validation failed - RoleName: {RoleName}, Errors: {Errors}", 
                    roleAddDto.RoleName, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                throw new ValidationException(validationResult.Errors);
            }

            var existingRole = await _roleRepository.GetRoleByNameAsync(roleAddDto.RoleName);
            if (existingRole != null)
            {
                _logger.LogWarning("Role name already exists validation failed - RoleName: {RoleName}", roleAddDto.RoleName);
                throw new Exception($"Role with name {roleAddDto.RoleName} already exists.");
            }
        }
        catch (Exception ex) when (ex is not ValidationException && 
                                   ex.Message != $"Role with name {roleAddDto.RoleName} already exists.")
        {
            _logger.LogError(ex, "Failed to validate role add request - RoleName: {RoleName}", roleAddDto.RoleName);
            throw;
        }
    }

    // This method validates if a role can be deleted.
    public async Task ValidateRoleCanBeDeletedAsync(int roleId)
    {
        try
        {
            await ValidateRoleExistAsync(roleId);
            
            var role = await _roleRepository.GetRoleByIdAsync(roleId);

            if (role!.RoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Admin role deletion attempt blocked - RoleId: {RoleId}", roleId);
                throw new Exception("Admin role cannot be deleted.");       
            }
            
            if (role!.RoleName.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("User role deletion attempt blocked - RoleId: {RoleId}", roleId);
                throw new Exception("User role cannot be deleted.");       
            }
        }
        catch (Exception ex) when (ex.Message != "Admin role cannot be deleted." && 
                                   ex.Message != "User role cannot be deleted.")
        {
            _logger.LogError(ex, "Failed to validate role deletion - RoleId: {RoleId}", roleId);
            throw;
        }    
    }

    // This method validates if a role can be removed from a user.
    public async Task ValidateRoleCanBeRemovedAsync(int userId, int roleId)
    {
       
        try
        {
            await ValidateRoleExistAsync(roleId);
        
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            var userRoles = await _userRoleRepository.GetRolesByUserIdAsync(userId);
            
            if (role!.RoleName.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("User role removal attempt blocked - UserId: {UserId}, RoleId: {RoleId}", userId, roleId);
                throw new Exception("User role cannot be removed. Every user must have the User role.");
            }
            
            if (role.RoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                var adminUsers = await _userRepository.GetUsersByRoleAsync("Admin", 1, 100);
                if (adminUsers.Count == 1 && adminUsers[0].UserId == userId)
                {
                    _logger.LogWarning("Last admin role removal attempt blocked - UserId: {UserId}, RoleId: {RoleId}", userId, roleId);
                    throw new Exception("Cannot remove Admin role from the last admin user.");
                }
            }
        }
        catch (Exception ex) when (ex.Message != "User role cannot be removed. Every user must have the User role." && 
                                   ex.Message != "Cannot remove Admin role from the last admin user.")
        {
            _logger.LogError(ex, "Failed to validate role removal - UserId: {UserId}, RoleId: {RoleId}", userId, roleId);
            throw;
        }
    }
}