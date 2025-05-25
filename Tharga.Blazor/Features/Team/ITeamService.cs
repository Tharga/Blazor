using System.Security.Claims;

namespace Tharga.Blazor.Features.Team;

public interface ITeamService
{
    IAsyncEnumerable<ITeam> GetTeamsAsync(ClaimsPrincipal currentUser);
    Task<ITeam> CreateTeamAsync(ClaimsPrincipal currentUser);
    IAsyncEnumerable<ITeamMember> GetTeamMembersAsync();
    Task DeleteTeamAsync(ClaimsPrincipal currentUser, ITeam team);
}