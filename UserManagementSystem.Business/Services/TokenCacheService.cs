using Microsoft.Extensions.Caching.Distributed;
using UserManagementSystem.Business.Interfaces;

namespace UserManagementSystem.Business.Services;

public class TokenCacheService: ITokenCacheService
{
    private readonly IDistributedCache _cache;
    
    public TokenCacheService(IDistributedCache  cache)
    {
        _cache = cache;
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
}