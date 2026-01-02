using System.Security.Claims;

namespace Tharga.Team;

public interface IUserService
{
    Task<IUser> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal = default);
    IAsyncEnumerable<IUser> GetAsync();
}