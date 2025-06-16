namespace Tharga.Blazor.Features.Team;

public interface ITeamService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;

    IAsyncEnumerable<ITeam> GetTeamsAsync();
    Task<ITeam> CreateTeamAsync();
    Task DeleteTeamAsync(ITeam team);
    Task RenameTeamAsync(ITeam team, string name);
}