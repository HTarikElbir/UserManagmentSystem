using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly ApplicationDbContext _context;
    
    public UserRoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AssignRoleToUserAsync(UserRole userRole)
    {
        await _context.UserRoles.AddAsync(userRole);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRoleFromUserAsync(UserRole userRole)
    {
        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();
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