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
            _logger.LogInformation("Resetting password for user: {UserId}", userId);
            
            var user = await _context.Users.FindAsync(userId);
            user!.Password = password;
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Password reset successfully for user: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reset password for user: {UserId}", userId);
            throw;
        }
    }
}