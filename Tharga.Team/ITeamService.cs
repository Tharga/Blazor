using System.Security.Claims;

namespace Tharga.Team;

public interface ITeamService<T> : ITeamService
    where T : ITeam
{
    Task<T> GetTeamAsync(string teamKey);
    Task UpdateTeamAsync(T team);
}

public interface ITeamService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;

    IAsyncEnumerable<ITeam<TMember>> GetAllTeamsAsync<TMember>(ClaimsPrincipal claimsPrincipal = null) where TMember : ITeamMember;
    IAsyncEnumerable<ITeam> GetTeamsAsync(ClaimsPrincipal claimsPrincipal = null);
    Task<ITeam> CreateTeamAsync();
    Task DeleteTeamAsync(string teamKey);
    IAsyncEnumerable<string> GetRolesAsync(ITeam team);
    Task SetLastSeenAsync(ITeam team, ClaimsPrincipal claimsPrincipal = null);
}