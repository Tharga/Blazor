using System.Security.Claims;

namespace Tharga.Team;

public enum AccessLevel
{
    Owner,
    Administrator,
    User,
    Viewer
}

//public interface ITeamRepo<TTeam>
//    where TTeam : ITeam
//{
//    IAsyncEnumerable<TTeam> GetTeamsByUserAsync(string userKey);
//}

//public abstract class TeamServiceBase<TTeam> : ITeamService
//    where TTeam : ITeam
//{
//    private readonly ITeamRepo<TTeam> _teamRepo;

//    protected TeamServiceBase(ITeamRepo<TTeam> teamRepo)
//    {
//        _teamRepo = teamRepo;
//    }

//    public event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;
//    public event EventHandler<SelectTeamEventArgs> SelectTeamEvent;

//    public IAsyncEnumerable<ITeam> GetTeamsAsync(ClaimsPrincipal claimsPrincipal = null)
//    {
//        throw new NotImplementedException();
//    }

//    public IAsyncEnumerable<ITeam<TMember>> GetTeamsAsync<TMember>(ClaimsPrincipal claimsPrincipal = null) where TMember : ITeamMember
//    {
//        throw new NotImplementedException();
//    }

//    public Task<ITeam<TMember>> GetTeamAsync<TMember>(string teamKey) where TMember : ITeamMember
//    {
//        throw new NotImplementedException();
//    }

//    public Task<ITeam> CreateTeamAsync(ClaimsPrincipal claimsPrincipal = null)
//    {
//        throw new NotImplementedException();
//    }

//    public Task RenameTeamAsync(string teamKey, string name, ClaimsPrincipal claimsPrincipal = null)
//    {
//        throw new NotImplementedException();
//    }

//    public Task DeleteTeamAsync(string teamKey, ClaimsPrincipal claimsPrincipal = null)
//    {
//        throw new NotImplementedException();
//    }

//    public async Task<ITeamMember> GetTeamMemberAsync(string teamKey, string userKey, ClaimsPrincipal claimsPrincipal = null)
//    {
//        var team = await _teamRepo.GetTeamsByUserAsync(userKey).FirstOrDefaultAsync(x => x.Key == teamKey);
//        return team?.Members.FirstOrDefault(x => x.Key == userKey);
//    }

//    public Task AddTeamMemberAsync(string teamKey, InviteUserModel model)
//    {
//        throw new NotImplementedException();
//    }

//    public Task RemoveMemberAsync(string teamKey, string userKey)
//    {
//        throw new NotImplementedException();
//    }

//    public Task SetMemberRoleAsync(string teamKey, string userKey, AccessLevel accessLevel)
//    {
//        throw new NotImplementedException();
//    }

//    public Task SetInvitationResponseAsync(string teamKey, string userKey, string inviteCode, bool accept)
//    {
//        throw new NotImplementedException();
//    }

//    public Task SetLastSeenAsync(ITeam team, ClaimsPrincipal claimsPrincipal = null)
//    {
//        throw new NotImplementedException();
//    }
//}