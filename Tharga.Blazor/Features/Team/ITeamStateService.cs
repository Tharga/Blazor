namespace Tharga.Blazor.Features.Team;

public interface ITeamStateService
{
    event EventHandler<TeamChangeEventArgs> TeamChangeEvent;

    ITeam SelectedTeam { get; }
    void SetCurrentAndRefresh(ITeam team);
    void OnTeamChangeEvent();
}