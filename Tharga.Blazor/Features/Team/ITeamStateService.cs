namespace Tharga.Blazor.Features.Team;

public interface ITeamStateService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;
    event EventHandler<SelectedTeamChangedEventArgs> SelectedTeamChangedEvent;

    Task<ITeam> GetSelectedTeamAsync();
    Task<string> GetSelectedTeamKeyAsync();
    Task SetSelectedTeamAsync(ITeam selectedTeam);
}