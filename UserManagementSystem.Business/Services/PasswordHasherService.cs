using Microsoft.Extensions.Logging;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.Business.Services;

public class PasswordHasherService : IPasswordHasher
{
    private readonly ILogger<PasswordHasherService> _logger;
    
    public PasswordHasherService(ILogger<PasswordHasherService> logger)
    {
        _logger = logger;
    }
    
    // Used to hash the password
    public string HashPassword(string password)
    {
        try
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Password hashing failed");
            throw;
        }
    }

    // Used to verify the hashed password
    public bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Password verification failed");
            throw;
        }
    }
}