using System.Collections.Concurrent;
using System.Security.Claims;
using Tharga.Toolkit;

namespace Tharga.Team;

public abstract class UserServiceBase : IUserService
{
    private static readonly ConcurrentDictionary<string, IUser> _userCache = new();

    protected abstract Task<ClaimsPrincipal> GetClaims(ClaimsPrincipal claimsPrincipal);
    protected abstract Task<IUser> GetUserAsync(ClaimsPrincipal claimsPrincipal);
    protected abstract IAsyncEnumerable<IUser> GetAllAsync();

    public async Task<IUser> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal)
    {
        claimsPrincipal = await GetClaims(claimsPrincipal);
        var identity = claimsPrincipal.GetIdentity().Identity;
        if (identity == null) return null;

        if (_userCache.TryGetValue(identity, out var user)) return user;

        var userEntity = await GetUserAsync(claimsPrincipal);

        _userCache.TryAdd(identity, userEntity);

        return userEntity;
    }

    public virtual IAsyncEnumerable<IUser> GetAsync()
    {
        return GetAllAsync();
    }
}