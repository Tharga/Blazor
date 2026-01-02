using System.Collections.Concurrent;
using Tharga.Toolkit;

namespace Tharga.Team;

public interface ITeamService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;
    event EventHandler<SelectTeamEventArgs> SelectTeamEvent;

    IAsyncEnumerable<ITeam> GetTeamsAsync();
    IAsyncEnumerable<ITeam<TMember>> GetTeamsAsync<TMember>() where TMember : ITeamMember;
    Task<ITeam<TMember>> GetTeamAsync<TMember>(string teamKey) where TMember : ITeamMember;
    Task<ITeam> CreateTeamAsync(string name = null);
    Task RenameTeamAsync<TMember>(string teamKey, string name) where TMember : ITeamMember;
    Task DeleteTeamAsync<TMember>(string teamKey) where TMember : ITeamMember;
    Task<ITeamMember> GetTeamMemberAsync(string teamKey, string userKey);
    Task AddTeamMemberAsync(string teamKey, InviteUserModel model);
    Task RemoveMemberAsync(string teamKey, string userKey);
    Task SetMemberRoleAsync(string teamKey, string userKey, AccessLevel accessLevel);
    Task SetInvitationResponseAsync(string teamKey, string userKey, string inviteCode, bool accept);
    Task SetLastSeenAsync(string teamKey);
}

public abstract class TeamServiceBase : ITeamService
{
    private readonly IUserService _userService;
    private static readonly ConcurrentDictionary<string, ITeamMember> _teamMemberCache = new();

    protected TeamServiceBase(IUserService userService)
    {
        _userService = userService;
    }

    public event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;
    public event EventHandler<SelectTeamEventArgs> SelectTeamEvent;

    public async IAsyncEnumerable<ITeam> GetTeamsAsync()
    {
        var user = await GetCurrentUserAsync();

        await foreach (var team in GetTeamsAsync(user))
        {
            yield return team;
        }
    }

    public async IAsyncEnumerable<ITeam<TMember>> GetTeamsAsync<TMember>() where TMember : ITeamMember
    {
        var user = await GetCurrentUserAsync();

        await foreach (var team in GetTeamsAsync(user))
        {
            yield return (ITeam<TMember>)team;
        }
    }

    public async Task<ITeam<TMember>> GetTeamAsync<TMember>(string teamKey) where TMember : ITeamMember
    {
        var team = await GetTeamAsync(teamKey);
        return (ITeam<TMember>)team;
    }

    protected abstract IAsyncEnumerable<ITeam> GetTeamsAsync(IUser user);
    protected abstract Task<ITeam> GetTeamAsync(string teamKey);
    protected abstract Task<ITeam> CreateTeamAsync(string teamKey, string name, IUser user);
    protected abstract Task SetTeamNameAsync(string teamKey, string name);
    protected abstract Task DeleteTeamAsyncA(string teamKey);
    public abstract Task AddTeamMemberAsync(string teamKey, InviteUserModel model);
    protected abstract Task RemoveMemberAsyncA(string teamKey, string userKey);
    protected abstract Task<ITeam> SetInvitationResponseAsyncA(string teamKey, string userKey, string inviteKey, bool accept);
    protected abstract Task SetLastSeenAsync(string teamKey, string userKey);
    protected abstract Task<ITeamMember> GetTeamMemberAsyncA(string teamKey, string userKey);
    protected abstract Task SetMemberRoleAsyncA(string teamKey, string userKey, AccessLevel accessLevel);

    public async Task<ITeamMember> GetTeamMemberAsync(string teamKey, string userKey)
    {
        var key = $"{teamKey}.{userKey}";
        if (_teamMemberCache.TryGetValue(key, out var teamMember)) return teamMember;

        teamMember = await GetTeamMemberAsyncA(teamKey, userKey);

        _teamMemberCache.TryAdd(key, teamMember);

        return teamMember;
    }

    public async Task<ITeam> CreateTeamAsync(string name)
    {
        var user = await GetCurrentUserAsync();

        var nameBuilder = new Lazy<string>(() =>
        {
            var email = user.EMail;
            var username = "Unknown";
            if (!string.IsNullOrEmpty(email))
            {
                var atIndex = email.IndexOf('@');
                username = atIndex >= 0 ? email.Substring(0, atIndex) : email;
            }

            return $"{username.Replace(".", " ")}'s team";
        });
        name ??= nameBuilder.Value;

        //var teamKey = StringExtension.UpperCaseAlphaNumericCharacters.Random();
        var teamKey = await GetRandomUnsusedTeamKey();

        var team = await CreateTeamAsync(teamKey, name, user);

        TeamsListChangedEvent?.Invoke(this, new TeamsListChangedEventArgs());
        SelectTeamEvent?.Invoke(this, new SelectTeamEventArgs(team));

        return team;
    }

    public async Task RenameTeamAsync<TMember>(string teamKey, string name) where TMember : ITeamMember
    {
        await AssureAccessLevel<TMember>(teamKey, AccessLevel.Administrator);

        await SetTeamNameAsync(teamKey, name);

        TeamsListChangedEvent?.Invoke(this, new TeamsListChangedEventArgs());
    }

    public async Task DeleteTeamAsync<TMember>(string teamKey) where TMember : ITeamMember
    {
        await AssureAccessLevel<TMember>(teamKey, AccessLevel.Administrator);

        await DeleteTeamAsyncA(teamKey);

        TeamsListChangedEvent?.Invoke(this, new TeamsListChangedEventArgs());
    }

    public async Task RemoveMemberAsync(string teamKey, string userKey)
    {
        await RemoveMemberAsyncA(teamKey, userKey);
        _teamMemberCache.TryRemove($"{teamKey}.{userKey}", out _);
        TeamsListChangedEvent?.Invoke(this, new TeamsListChangedEventArgs());
    }

    public async Task SetMemberRoleAsync(string teamKey, string userKey, AccessLevel accessLevel)
    {
        await SetMemberRoleAsyncA(teamKey, userKey, accessLevel);
        _teamMemberCache.TryRemove($"{teamKey}.{userKey}", out _);
    }

    public async Task SetInvitationResponseAsync(string teamKey, string userKey, string inviteKey, bool accept)
    {
        if (accept)
        {
            var team = await SetInvitationResponseAsyncA(teamKey, userKey, inviteKey, true);
            TeamsListChangedEvent?.Invoke(this, new TeamsListChangedEventArgs());
            SelectTeamEvent?.Invoke(this, new SelectTeamEventArgs(team));
        }
        else
        {
            await SetInvitationResponseAsyncA(teamKey, userKey, inviteKey, false);
        }

        _teamMemberCache.TryRemove($"{teamKey}.{userKey}", out _);
    }

    public async Task SetLastSeenAsync(string teamKey)
    {
        var user = await GetCurrentUserAsync();
        await SetLastSeenAsync(teamKey, user.Key);
        _teamMemberCache.TryRemove($"{teamKey}.{user.Key}", out _);
    }

    private async Task<string> GetRandomUnsusedTeamKey()
    {
        string teamKey;
        while (true)
        {
            teamKey = StringExtension.UpperCaseAlphaNumericCharacters.Random();
            var item = await GetTeamAsync(teamKey);
            if (item == null) break;
        }

        return teamKey;
    }

    private async Task AssureAccessLevel<TMember>(string teamKey, AccessLevel accessLevel) where TMember : ITeamMember
    {
        var user = await GetCurrentUserAsync();
        var team = await GetTeamAsync<TMember>(teamKey);
        var member = team.Members.Single(x => x.Key == user.Key);
        if (member.State != MembershipState.Member) throw new InvalidOperationException("User is not a member.");
        if (member.AccessLevel > accessLevel) throw new InvalidOperationException($"Cannot be executed by user {user.EMail} with {member.AccessLevel}.");
    }

    private async Task<IUser> GetCurrentUserAsync()
    {
        var user = await _userService.GetCurrentUserAsync();
        return user;
    }
}