namespace UserManagementSystem.Business.Interfaces;

public interface ITokenCacheService
{
    Task SetTokenAsync(string key, string value, TimeSpan? expiry = null);
    Task<string> GetTokenAsync(string key);
    Task RemoveTokenAsync(string key);
}