namespace Tharga.Blazor.Features.Team;

public interface ITeamStateService
{
    //event EventHandler<FarmChangedEventArgs> FarmChangedEvent;
    //event EventHandler<EventArgs> RequestReloadEvent;
    ITeam SelectedTeam { get; }
    //void SetCurrent(TeamModel farmDto);
    void SetCurrentAndRefresh(ITeam team);
    //void OverrideRedirect(Action action);
}