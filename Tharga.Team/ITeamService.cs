using System.Security.Claims;

namespace Tharga.Team;

public interface IUserService
{
    Task<IUser> GetOrCreateAsync(ClaimsPrincipal claimsPrincipal);
    Task<IUser> GetAsync(string identity);
}

public interface ITeamService<T> : ITeamService
    where T : ITeam
{
    //TODO: Revisit -->

    Task<T> GetTeamAsync(string teamKey);
    Task UpdateTeamAsync(T team);
}

public abstract class TeamServiceBase
{
}

public interface ITeamService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;

    IAsyncEnumerable<ITeam> GetTeamsAsync(ClaimsPrincipal claimsPrincipal = null);
    IAsyncEnumerable<ITeam<TMember>> GetTeamsAsync<TMember>(ClaimsPrincipal claimsPrincipal = null) where TMember : ITeamMember;
    Task<ITeamMember> GetTeamMemberAsync(string teamKey, string userKey, ClaimsPrincipal claimsPrincipal = null);
    Task<ITeam> CreateTeamAsync(ClaimsPrincipal claimsPrincipal = null);
    Task SetLastSeenAsync(ITeam team, ClaimsPrincipal claimsPrincipal = null);
    Task RenameTeamAsync(string teamKey, string name, ClaimsPrincipal claimsPrincipal = null);
    Task DeleteTeamAsync(string teamKey, ClaimsPrincipal claimsPrincipal = null);

    //TODO: Revisit -->

    IAsyncEnumerable<string> GetRolesAsync(ITeam team);
}