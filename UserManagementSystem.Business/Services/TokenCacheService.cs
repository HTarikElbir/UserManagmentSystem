using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using UserManagementSystem.Business.Interfaces;
using UserManagementSystem.Business.Settings;

namespace UserManagementSystem.Business.Services;

public class TokenCacheService: ITokenCacheService
{
    private readonly IDistributedCache _cache;
    private readonly JwtSettings _jwtSettings;
    private const string BlackListKey = "blacklist:";
    public TokenCacheService(IDistributedCache  cache, IOptions<JwtSettings> jwtSettings)
    {
        _cache = cache;
        _jwtSettings = jwtSettings.Value;
    }
    
    public async Task SetTokenAsync(string key, string value, TimeSpan? expiry = null)
    {
        var options = new DistributedCacheEntryOptions();
        if (expiry.HasValue)
            options.AbsoluteExpirationRelativeToNow = expiry;

        await _cache.SetStringAsync(key, value, options);
    }
    
    public async  Task<string?> GetTokenAsync(string key)
    {
        return await _cache.GetStringAsync(key);
    }
    
    public async Task RemoveTokenAsync(string key)
    { 
        await _cache.RemoveAsync(key);
    }
    
    public async Task AddToBlackListAsync(string token)
    {
        var key = $"{BlackListKey}{token}";

        var options = new DistributedCacheEntryOptions();

        options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_jwtSettings.ExpireMinutes);
        
        await _cache.SetStringAsync(key, "blacklisted", options);
    }

    public async  Task<bool> IsInBlackListAsync(string token)
    {
        var key = $"{BlackListKey}{token}";
        
        var blackListedToken = await _cache.GetStringAsync(key);
        
        return !string.IsNullOrEmpty(blackListedToken);
    }
}