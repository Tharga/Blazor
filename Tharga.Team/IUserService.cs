using System.Security.Claims;

namespace Tharga.Team;

public interface IUserService
{
    Task<IUser> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal = null);
    IAsyncEnumerable<IUser> GetAsync();
}