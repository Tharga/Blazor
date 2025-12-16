namespace Tharga.Team;

public class SelectedTeamChangedEventArgs : EventArgs
{
    public SelectedTeamChangedEventArgs(ITeam selectedTeam)
    {
        SelectedTeam = selectedTeam;
    }

    public ITeam SelectedTeam { get; }
}