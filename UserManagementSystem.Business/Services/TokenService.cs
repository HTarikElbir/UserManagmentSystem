using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Settings;
using UserManagementSystem.Data.Entities;
using UserManagementSystem.Data.Interfaces;

namespace UserManagementSystem.Business.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    
    
    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    
    // Creates a JWT token for the user
    public string CreateToken(User user)
    {
        var claims = GetUserClaims(user);
        return GenerateToken(claims);
    }

    // Creates a JWT token for resetting the password
    public string CreateResetPasswordToken(string email)
    {
        var claims = GetResetPasswordClaims(email);
        return  GenerateResetPasswordToken(claims);
    }

    // Extracts claims from the user object
    private List<Claim> GetUserClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        foreach (var userRole in user.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
        }

        return claims;
    }
    
    // Extracts claims for password reset
    private  List<Claim> GetResetPasswordClaims(string email)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim("TokenType", "ResetPassword")
        };
        
        return claims;
    }
    
    // Generates a JWT token for login
    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = _jwtSettings.SecretKey;
        var issuer = _jwtSettings.Issuer;
        var audience = _jwtSettings.Audience;
        var expireMinutes = _jwtSettings.ExpireMinutes;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(expireMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Generates a JWT token for password reset
    private string GenerateResetPasswordToken(IEnumerable<Claim> claims)
    {
        var key = _jwtSettings.SecretKey;
        var issuer = _jwtSettings.Issuer;
        var audience = _jwtSettings.Audience;
        var expireMinutes = 15;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
    
   