using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class AuthRepository: IAuthRepository
{
    private readonly ApplicationDbContext _context;
        
    public AuthRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // This method checks if the user exists in the database and verifies the password.
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async  Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ResetPasswordAsync(int userId, string password)
    {
        var user = await _context.Users.FindAsync(userId);
        
        user!.Password = password;
        await _context.SaveChangesAsync();
        
        return true;
    }

    public Task<User?> GetUserByIdAsync(int userId)
    {
        return _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }
}