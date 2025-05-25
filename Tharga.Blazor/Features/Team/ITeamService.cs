using System.Security.Claims;

namespace Tharga.Blazor.Features.Team;

public interface ITeamService
{
    IAsyncEnumerable<ITeam> GetTeamsAsync(ClaimsPrincipal currentUser);
    Task<ITeam> CreateTeamAsync(ClaimsPrincipal currentUser);
    Task DeleteTeamAsync(ClaimsPrincipal currentUser, ITeam team);
    Task RenameTeamAsync(ClaimsPrincipal currentUser, ITeam team, string name);
}