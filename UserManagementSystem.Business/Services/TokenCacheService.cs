using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Settings;

namespace UserManagementSystem.Business.Services;

public class TokenCacheService: ITokenCacheService
{
    private readonly IDistributedCache _cache;
    private readonly JwtSettings _jwtSettings;
    private const string BlackListKey = "blacklist:";
    private readonly ILogger<TokenCacheService> _logger;
    public TokenCacheService(IDistributedCache  cache, IOptions<JwtSettings> jwtSettings, ILogger<TokenCacheService> logger)
    {
        _cache = cache;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }
    
    public async Task SetTokenAsync(string key, string value, TimeSpan? expiry = null)
    {
        try
        {
            _logger.LogDebug("Setting token in cache - Key: {Key}, Expiry: {Expiry}", key, expiry);
            
            var options = new DistributedCacheEntryOptions();
            if (expiry.HasValue)
                options.AbsoluteExpirationRelativeToNow = expiry;

            await _cache.SetStringAsync(key, value, options);
            
            _logger.LogDebug("Token set in cache successfully - Key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set token in cache - Key: {Key}", key);
            throw;
        }
    }
    
    public async  Task<string?> GetTokenAsync(string key)
    {
        try
        {
            _logger.LogDebug("Getting token from cache - Key: {Key}", key);
            
            var token = await _cache.GetStringAsync(key);
            
            if (token != null)
            {
                _logger.LogDebug("Token retrieved from cache - Key: {Key}", key);
            }
            else
            {
                _logger.LogDebug("Token not found in cache - Key: {Key}", key);
            }
            
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get token from cache - Key: {Key}", key);
            throw;
        }
    }
    
    public async Task RemoveTokenAsync(string key)
    { 
        try
        {
            _logger.LogDebug("Removing token from cache - Key: {Key}", key);
            
            await _cache.RemoveAsync(key);
            
            _logger.LogDebug("Token removed from cache successfully - Key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove token from cache - Key: {Key}", key);
            throw;
        }
    }
    
    public async Task AddToBlackListAsync(string token)
    {
        try
        {
            _logger.LogInformation("Adding token to blacklist");
        
            var key = $"{BlackListKey}{token}";
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_jwtSettings.ExpireMinutes);
        
            await _cache.SetStringAsync(key, "blacklisted", options);
        
            _logger.LogInformation("Token added to blacklist successfully, Expiry: {ExpiryMinutes} minutes", _jwtSettings.ExpireMinutes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add token to blacklist");
            throw;
        }
    }

    public async  Task<bool> IsInBlackListAsync(string token)
    {
        try
        {
            _logger.LogDebug("Checking if token is in blacklist");
        
            var key = $"{BlackListKey}{token}";
            var blackListedToken = await _cache.GetStringAsync(key);
            var isBlacklisted = !string.IsNullOrEmpty(blackListedToken);
        
            if (isBlacklisted)
            {
                _logger.LogDebug("Token found in blacklist");
            }
            else
            {
                _logger.LogDebug("Token not found in blacklist");
            }
        
            return isBlacklisted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check if token is in blacklist");
            throw;
        }
    }
}