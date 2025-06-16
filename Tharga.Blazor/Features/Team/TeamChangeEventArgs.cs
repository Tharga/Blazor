namespace Tharga.Blazor.Features.Team;

public class SelectedTeamChangedEventArgs : EventArgs
{
    public SelectedTeamChangedEventArgs(ITeam selectedTeam)
    {
        SelectedTeam = selectedTeam;
    }

    public ITeam SelectedTeam { get; }
}

public class TeamsListChangedEventArgs : EventArgs
{
}