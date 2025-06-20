using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserRoleRepository> _logger;
    public UserRoleRepository(ApplicationDbContext context, ILogger<UserRoleRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task AssignRoleToUserAsync(UserRole userRole)
    {
        try
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error during role assignment - UserId: {UserId}, RoleId: {RoleId}", 
                userRole.UserId, userRole.RoleId);
            throw;
        }
    }

    public async Task RemoveRoleFromUserAsync(UserRole userRole)
    {
        try
        {
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error during role removal - UserId: {UserId}, RoleId: {RoleId}", 
                userRole.UserId, userRole.RoleId);
            throw;
        }
    }

    public async Task<List<string>> GetRolesByUserIdAsync(int userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .Select(ur => ur.Role.RoleName)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<User>> GetUsersByRoleIdAsync(int roleId)
    {
        return await _context.UserRoles
            .Where(ur => ur.RoleId == roleId)
            .Include(ur => ur.User)
            .Select(ur => ur.User)
            .AsNoTracking()
            .ToListAsync();
    }
}