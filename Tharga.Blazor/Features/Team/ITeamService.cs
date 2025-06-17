namespace Tharga.Blazor.Features.Team;

public interface ITeamService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;

    IAsyncEnumerable<ITeam> GetTeamsAsync();
    Task<ITeam> CreateTeamAsync();
    Task DeleteTeamAsync(string teamKey);
    //Task RenameTeamAsync(string teamKey, string name);
    //Task AddUserAsync();
}

public interface ITeamService<T> : ITeamService
    where T : ITeam
{
    Task<T> GetTeamAsync(string teamKey);
    Task UpdateTeamAsync(T team);
}