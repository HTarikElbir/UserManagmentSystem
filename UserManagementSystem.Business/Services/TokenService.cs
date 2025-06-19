using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
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
    private readonly ITokenCacheService _tokenCacheService;
    private readonly ILogger<TokenService> _logger;
    
    public TokenService(IOptions<JwtSettings> jwtSettings, ITokenCacheService tokenCacheService, ILogger<TokenService> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _tokenCacheService = tokenCacheService;
        _logger = logger;
    }
    
    // Creates a JWT token for the user
    public string CreateToken(User user)
    {
        try
        {
            _logger.LogInformation("Creating JWT token for user: {UserId}, Username: {Username}", user.UserId, user.UserName);
            
            var claims = GetUserClaims(user);
            var token = GenerateToken(claims);
            
            _logger.LogInformation("JWT token created successfully for user: {UserId}", user.UserId);
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create JWT token for user: {UserId}", user.UserId);
            throw;
        }
    }

    // Creates a JWT token for resetting the password
    public string CreateResetPasswordToken(string email)
    {
        try
        {
            _logger.LogInformation("Creating reset password token for email: {Email}", email);
            
            var claims = GetResetPasswordClaims(email);
            var token = GenerateResetPasswordToken(claims);
            
            _logger.LogInformation("Reset password token created successfully for email: {Email}", email);
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create reset password token for email: {Email}", email);
            throw;
        }
    }

    // Validates tokens
    public async Task<bool> ValidateToken(string token, int userId )
    {
        try
        {
            _logger.LogDebug("Validating token for user: {UserId}", userId);
            
            if(await _tokenCacheService.IsInBlackListAsync(token))
            {
                _logger.LogWarning("Token is blacklisted for user: {UserId}", userId);
                return false;
            }
            
            var cachedToken = await _tokenCacheService.GetTokenAsync($"user:{userId}:login_token");

            var isValid = !string.IsNullOrEmpty(cachedToken) && string.Equals(cachedToken, token, StringComparison.Ordinal);
            
            if (isValid)
            {
                _logger.LogDebug("Token validation successful for user: {UserId}", userId);
            }
            else
            {
                _logger.LogWarning("Token validation failed for user: {UserId}", userId);
            }
            
            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate token for user: {UserId}", userId);
            throw;
        }
    }

    // Extracts claims from the user object
    private List<Claim> GetUserClaims(User user)
    {
        try
        {
            _logger.LogDebug("Extracting claims for user: {UserId}", user.UserId);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
                _logger.LogDebug("Added role claim: {RoleName} for user: {UserId}", userRole.Role.RoleName, user.UserId);
            }

            _logger.LogDebug("Extracted {ClaimCount} claims for user: {UserId}", claims.Count, user.UserId);
            return claims;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract claims for user: {UserId}", user.UserId);
            throw;
        }
    }
    
    // Extracts claims for password reset
    private  List<Claim> GetResetPasswordClaims(string email)
    {
        try
        {
            _logger.LogDebug("Extracting reset password claims for email: {Email}", email);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("TokenType", "ResetPassword")
            };
            
            _logger.LogDebug("Extracted reset password claims for email: {Email}", email);
            return claims;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to extract reset password claims for email: {Email}", email);
            throw;
        }
    }
    
    // Generates a JWT token for login
    private string GenerateToken(IEnumerable<Claim> claims)
    {
        try
        {
            _logger.LogDebug("Generating JWT token with {ClaimCount} claims", claims.Count());
            
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

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            
            _logger.LogDebug("JWT token generated successfully, expires in {ExpireMinutes} minutes", expireMinutes);
            return tokenString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate JWT token");
            throw;
        }
    }

    // Generates a JWT token for password reset
    private string GenerateResetPasswordToken(IEnumerable<Claim> claims)
    {
        try
        {
            _logger.LogDebug("Generating reset password JWT token with {ClaimCount} claims", claims.Count());
            
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

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            
            _logger.LogDebug("Reset password JWT token generated successfully, expires in {ExpireMinutes} minutes", expireMinutes);
            return tokenString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate reset password JWT token");
            throw;
        }
    }
    
    
}
    
   