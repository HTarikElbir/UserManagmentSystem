using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using UserManagementSystem.Business.Dtos;
using UserManagementSystem.Business.Dtos.Role;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Interfaces.Validation;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

// Service class for role operations
public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly IRoleValidationService _roleValidationService;
    private readonly ILogger<RoleService> _logger;

    public RoleService(IRoleRepository roleRepository, 
        IMapper mapper, 
        IRoleValidationService roleValidationService,
        ILogger<RoleService> logger)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
        _roleValidationService = roleValidationService;
        _logger = logger;
    }
    
    // Retrieves all roles
    public async Task<List<RoleDto>> GetAllRolesAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Getting all roles - Page: {Page}, PageSize: {PageSize}", page, pageSize);
            
            var roles = await _roleRepository.GetAllRolesAsync(page, pageSize);
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);
            
            _logger.LogInformation("Retrieved {Count} roles", roleDtos.Count);
            return roleDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all roles - Page: {Page}, PageSize: {PageSize}", page, pageSize);
            throw;
        }
    }
    
    // Retrieve the role by ID
    public async Task<RoleDto?> GetRoleByIdAsync(int roleId)
    {
        try
        {
            _logger.LogInformation("Getting role by ID: {RoleId}", roleId);
            
            await _roleValidationService.ValidateRoleExistAsync(roleId);
            
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            var roleDto = _mapper.Map<RoleDto>(role);
            
            _logger.LogInformation("Role retrieved successfully: {RoleId}, Name: {RoleName}", roleId, roleDto.RoleName);
            return roleDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get role by ID: {RoleId}", roleId);
            throw;
        }
    }
    
    // Retrieve the role by name
    public async Task<RoleDto?> GetRoleByNameAsync(string roleName)
    {
        try
        {
            _logger.LogInformation("Getting role by name: {RoleName}", roleName);
            
            var role = await _roleRepository.GetRoleByNameAsync(roleName);
            var roleDto = _mapper.Map<RoleDto>(role);
            
            if (roleDto != null)
            {
                _logger.LogInformation("Role found by name: {RoleName}, RoleId: {RoleId}", roleName, roleDto.RoleId);
            }
            else
            {
                _logger.LogInformation("Role not found by name: {RoleName}", roleName);
            }
            
            return roleDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get role by name: {RoleName}", roleName);
            throw;
        }
    }

    // Adds a new role
    public async Task<bool> AddRoleAsync(RoleAddDto roleAddDto)
    {
        try
        {
            _logger.LogInformation("Adding new role: {RoleName}", roleAddDto.RoleName);
            
            await _roleValidationService.ValidateAddRequestAsync(roleAddDto);
            
            var role = _mapper.Map<Role>(roleAddDto);
            await _roleRepository.AddRoleAsync(role);
            
            _logger.LogInformation("Role added successfully: {RoleName}, RoleId: {RoleId}", roleAddDto.RoleName, role.RoleId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add role: {RoleName}", roleAddDto.RoleName);
            throw;
        }
    }
    
    // Updates an existing role
    public async Task<bool> UpdateRoleAsync(int roleId, RoleUpdateDto roleUpdateDto)
    {
        try
        {
            _logger.LogInformation("Updating role: {RoleId}", roleId);
            
            await _roleValidationService.ValidateUpdateRequestAsync(roleId, roleUpdateDto);
            
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            _mapper.Map(roleUpdateDto, role);
            
            await _roleRepository.UpdateRoleAsync(role!);
            
            _logger.LogInformation("Role updated successfully: {RoleId}, NewName: {RoleName}", roleId, roleUpdateDto.RoleName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update role: {RoleId}", roleId);
            throw;
        }
    }
    
    // Deletes a role by its ID
    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        try
        {
            _logger.LogInformation("Deleting role: {RoleId}", roleId);
            
            await _roleValidationService.ValidateRoleCanBeDeletedAsync(roleId);
            await _roleRepository.DeleteRoleAsync(roleId);
            
            _logger.LogInformation("Role deleted successfully: {RoleId}", roleId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete role: {RoleId}", roleId);
            throw;
        }
    }
    
}