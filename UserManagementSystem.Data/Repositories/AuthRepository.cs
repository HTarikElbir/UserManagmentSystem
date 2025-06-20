using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagementSystem.Data.Contexts;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Data.Repositories;

public class AuthRepository: IAuthRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AuthRepository> _logger;
    
    public AuthRepository(ApplicationDbContext context, ILogger<AuthRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<bool> ResetPasswordAsync(int userId, string password)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            user!.Password = password;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database error during password reset for user: {UserId}", userId);
            throw;
        }
    }
}