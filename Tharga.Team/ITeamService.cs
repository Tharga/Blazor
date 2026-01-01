using System.Security.Claims;

namespace Tharga.Team;

public record InviteUserModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public AccessLevel AccessLevel { get; set; }
}

public interface IUserService
{
    Task<IUser> GetOrCreateAsync(ClaimsPrincipal claimsPrincipal);
    Task<IUser> GetAsync(ClaimsPrincipal claimsPrincipal);
    IAsyncEnumerable<IUser> GetAllAsync();
}

public interface ITeamService<T> : ITeamService
    where T : ITeam
{
    //TODO: Revisit -->

    Task<T> GetTeamAsync(string teamKey);
    Task UpdateTeamAsync(T team);
}

public record InviteModel
{
    public required string TeamKey { get; init; }
    public required string Code { get; init; }
}

public abstract class TeamServiceBase
{
}

public interface ITeamService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;
    event EventHandler<SelectTeamEventArgs> SelectTeamEvent;

    IAsyncEnumerable<ITeam> GetTeamsAsync(ClaimsPrincipal claimsPrincipal = null);
    IAsyncEnumerable<ITeam<TMember>> GetTeamsAsync<TMember>(ClaimsPrincipal claimsPrincipal = null) where TMember : ITeamMember;
    Task<ITeam<TMember>> GetTeamAsync<TMember>(string teamKey) where TMember : ITeamMember;
    Task<ITeamMember> GetTeamMemberAsync(string teamKey, string userKey, ClaimsPrincipal claimsPrincipal = null);
    Task AddTeamMemberAsync(string teamKey, InviteUserModel model);
    Task RemoveMemberAsync(string teamKey, string userKey);
    Task SetMemberRoleAsync(string teamKey, string userKey, AccessLevel accessLevel);
    Task SetInvitationResponseAsync(string teamKey, string userKey, string inviteCode, bool accept);
    Task<ITeam> CreateTeamAsync(ClaimsPrincipal claimsPrincipal = null);
    Task SetLastSeenAsync(ITeam team, ClaimsPrincipal claimsPrincipal = null);
    Task RenameTeamAsync(string teamKey, string name, ClaimsPrincipal claimsPrincipal = null);
    Task DeleteTeamAsync(string teamKey, ClaimsPrincipal claimsPrincipal = null);

    //TODO: Revisit -->

    IAsyncEnumerable<string> GetRolesAsync(ITeam team);
}