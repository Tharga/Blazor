namespace Tharga.Team;

public class SelectTeamEventArgs : EventArgs
{
    public SelectTeamEventArgs(ITeam team)
    {
        Team = team;
    }

    public ITeam Team { get; }
}