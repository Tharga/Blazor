namespace Tharga.Team;

public interface ITeamService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;
    event EventHandler<SelectTeamEventArgs> SelectTeamEvent;

    IAsyncEnumerable<ITeam> GetTeamsAsync();
    IAsyncEnumerable<ITeam<TMember>> GetTeamsAsync<TMember>() where TMember : ITeamMember;
    Task<ITeam<TMember>> GetTeamAsync<TMember>(string teamKey) where TMember : ITeamMember;
    Task<ITeam> CreateTeamAsync(string name = null);
    Task RenameTeamAsync(string teamKey, string name);
    Task DeleteTeamAsync(string teamKey);
    Task<ITeamMember> GetTeamMemberAsync(string teamKey, string userKey);
    Task AddTeamMemberAsync(string teamKey, InviteUserModel model);
    Task RemoveMemberAsync(string teamKey, string userKey);
    Task SetMemberRoleAsync(string teamKey, string userKey, AccessLevel accessLevel);
    Task SetInvitationResponseAsync(string teamKey, string userKey, string inviteCode, bool accept);
    Task SetLastSeenAsync(string teamKey);
}