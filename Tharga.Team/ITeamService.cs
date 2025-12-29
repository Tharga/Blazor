using System.Security.Claims;

namespace Tharga.Team;

public interface ITeamService<T> : ITeamService
    where T : ITeam
{
    //TODO: Revisit -->

    Task<T> GetTeamAsync(string teamKey);
    Task UpdateTeamAsync(T team);
}

//
public interface ITeamService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;

    IAsyncEnumerable<ITeam> GetTeamsAsync(ClaimsPrincipal claimsPrincipal = null);
    IAsyncEnumerable<ITeam<TMember>> GetTeamsAsync<TMember>(ClaimsPrincipal claimsPrincipal = null) where TMember : ITeamMember;
    Task<ITeam> CreateTeamAsync(ClaimsPrincipal claimsPrincipal = null);
    Task SetLastSeenAsync(ITeam team, ClaimsPrincipal claimsPrincipal = null);
    Task RenameTeamAsync(string teamKey, string name, ClaimsPrincipal claimsPrincipal = null);
    Task DeleteTeamAsync(string teamKey, ClaimsPrincipal claimsPrincipal = null);

    //TODO: Revisit -->

    IAsyncEnumerable<string> GetRolesAsync(ITeam team);
}