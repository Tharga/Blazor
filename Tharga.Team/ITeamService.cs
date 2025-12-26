using System.Security.Claims;

namespace Tharga.Team;

public interface ITeamService<T> : ITeamService
    where T : ITeam
{
    Task<T> GetTeamAsync(string teamKey);
    Task UpdateTeamAsync(T team);
}

//
public interface ITeamService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;

    Task RenameTeamAsync(string teamKey, string name);
    Task DeleteTeamAsync(string teamKey);

    //TODO: Revisit -->

    IAsyncEnumerable<ITeam<TMember>> GetAllTeamsAsync<TMember>(ClaimsPrincipal claimsPrincipal = null) where TMember : ITeamMember;
    IAsyncEnumerable<ITeam> GetTeamsAsync(ClaimsPrincipal claimsPrincipal = null);
    Task<ITeam> CreateTeamAsync(ClaimsPrincipal claimsPrincipal = null);
    IAsyncEnumerable<string> GetRolesAsync(ITeam team);
    Task SetLastSeenAsync(ITeam team, ClaimsPrincipal claimsPrincipal = null);
}