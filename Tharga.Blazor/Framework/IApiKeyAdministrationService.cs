namespace Tharga.Blazor.Framework;

public interface IApiKeyAdministrationService
{
    Task<IApiKey> GetByApiKeyAsync(string apiKey);
    IAsyncEnumerable<IApiKey> GetKeysAsync(string teamKey);
    Task RefreshKeyAsync(string teamKey, string key);
    Task LockKeyAsync(string key);
}