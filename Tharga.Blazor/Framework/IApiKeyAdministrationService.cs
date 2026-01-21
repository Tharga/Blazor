namespace Tharga.Blazor.Framework;

public interface IApiKeyAdministrationService
{
    IAsyncEnumerable<IApiKey> GetKeysAsync(string teamKey);
    Task RefreshKeyAsync(string teamKey, string key);
    Task LockKeyAsync(string key);
}