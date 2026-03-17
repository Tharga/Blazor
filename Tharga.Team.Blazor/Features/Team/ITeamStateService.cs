using Tharga.Team;

namespace Tharga.Team.Blazor.Features.Team;

public interface ITeamStateService
{
    event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;
    event EventHandler<SelectedTeamChangedEventArgs> SelectedTeamChangedEvent;

    Task<ITeam> GetSelectedTeamAsync();
    Task SetSelectedTeamAsync(ITeam selectedTeam);
}