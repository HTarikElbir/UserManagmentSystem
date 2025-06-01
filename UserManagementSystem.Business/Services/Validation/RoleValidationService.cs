using FluentValidation;
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
    
    
    public RoleValidationService(IRoleRepository roleRepository, 
        IValidator<RoleUpdateDto> updateValidator, 
        IValidator<RoleAddDto> addValidator,
        IUserRoleRepository userRoleRepository,
        IUserRepository userRepository)
    {
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;    
        _userRepository = userRepository;   
        _updateValidator = updateValidator;
        _addValidator = addValidator;
    }
    // Validates if a role exists by its ID
    public async Task ValidateRoleExistAsync(int roleId)
    { 
        var role = await _roleRepository.GetRoleByIdAsync(roleId);
        
        if (role == null)
        {
            throw new Exception($"Role with ID {roleId} does not exist.");
        }
    }
    
    // Validates the update request
    public async Task ValidateUpdateRequestAsync(int roleId, RoleUpdateDto roleUpdateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(roleUpdateDto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await ValidateRoleExistAsync(roleId);
    }

    public async Task ValidateAddRequestAsync(RoleAddDto roleAddDto)
    {
        var validationResult = await _addValidator.ValidateAsync(roleAddDto);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var existingRole = await _roleRepository.GetRoleByNameAsync(roleAddDto.RoleName);
        if (existingRole != null)
        {
            throw new Exception($"Role with name {roleAddDto.RoleName} already exists.");
        }
    }

    // This method validates if a role can be deleted.
    public async Task ValidateRoleCanBeDeletedAsync(int roleId)
    {
        await ValidateRoleExistAsync(roleId);
        
        var role = await _roleRepository.GetRoleByIdAsync(roleId);

        if (role!.RoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            throw new Exception("Admin role cannot be deleted.");       
        
        if (role!.RoleName.Equals("User", StringComparison.OrdinalIgnoreCase))
            throw new Exception("User role cannot be deleted.");       
    }

    // This method validates if a role can be removed from a user.
    public async Task ValidateRoleCanBeRemovedAsync(int userId, int roleId)
    {
        await ValidateRoleExistAsync(roleId);
    
        var role = await _roleRepository.GetRoleByIdAsync(roleId);
        var userRoles = await _userRoleRepository.GetRolesByUserIdAsync(userId);
        
        if (role!.RoleName.Equals("User", StringComparison.OrdinalIgnoreCase))
        {
            throw new Exception("User role cannot be removed. Every user must have the User role.");
        }
        
        if (role.RoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            var adminUsers = await _userRepository.GetUsersByRoleAsync("Admin", 1, 100);
            if (adminUsers.Count == 1 && adminUsers[0].UserId == userId)
            {
                throw new Exception("Cannot remove Admin role from the last admin user.");
            }
        }
    }
}