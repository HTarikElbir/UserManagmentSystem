using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.Business.Services;

public class PasswordHasherService : IPasswordHasher
{
    // Used to hash the password
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Used to verify the hashed password
    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}