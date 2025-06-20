using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Extensions;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RoleRepository> _logger;
    
    public RoleRepository(ApplicationDbContext context, ILogger<RoleRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    // Returns a list of all roles in the system.
    public async Task<List<Role>> GetAllRolesAsync(int page = 1, int pageSize = 10) => await  _context.Roles
        .AsNoTracking()
        .Paginate(page,pageSize)
        .ToListAsync();
    
    // Returns a role by id
    public async Task<Role?> GetRoleByIdAsync(int roleId) => await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);

    // Returns a role by name
    public async Task<Role?> GetRoleByNameAsync(string roleName) => await  _context.Roles
        .AsNoTracking()
        .FirstOrDefaultAsync(r => r.RoleName.ToLower() == roleName.ToLower());
    
    // Adds a new role to the database. 
    public async Task AddRoleAsync(Role role)
    {
        try
        {
            _logger.LogInformation("Adding new role: {RoleName}", role.RoleName);
            
            await _context.Roles.AddAsync(role); 
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Role added successfully: {RoleName}, RoleId: {RoleId}", role.RoleName, role.RoleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add role: {RoleName}", role.RoleName);
            throw;
        }
    }
    
    // Updates an existing role's data.
    public async Task UpdateRoleAsync(Role role)
    {
        try
        {
            _logger.LogInformation("Updating role: {RoleId}, RoleName: {RoleName}", role.RoleId, role.RoleName);
            
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Role updated successfully: {RoleId}", role.RoleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update role: {RoleId}", role.RoleId);
            throw;
        }
    }

    // Deletes a role by ID if found.
    public async Task DeleteRoleAsync(int roleId)
    {
        try
        {
            _logger.LogInformation("Deleting role: {RoleId}", roleId);
        
            var role = await _context.Roles.FindAsync(roleId);
            _context.Roles.Remove(role!);
            await _context.SaveChangesAsync();
        
            _logger.LogInformation("Role deleted successfully: {RoleId}", roleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete role: {RoleId}", roleId);
            throw;
        }
    }
}