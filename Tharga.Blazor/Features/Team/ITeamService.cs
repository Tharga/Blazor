using System.Collections.Generic;
using System.Security.Claims;

namespace Tharga.Blazor.Features.Team;

public interface ITeamService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;

    IAsyncEnumerable<ITeam> GetTeamsAsync(ClaimsPrincipal claimsPrincipal = null);
    Task<ITeam> CreateTeamAsync();
    Task DeleteTeamAsync(string teamKey);
    IAsyncEnumerable<string> GetRolesAsync(ITeam team);
}

public interface ITeamService<T> : ITeamService
    where T : ITeam
{
    Task<T> GetTeamAsync(string teamKey);
    Task UpdateTeamAsync(T team);
}