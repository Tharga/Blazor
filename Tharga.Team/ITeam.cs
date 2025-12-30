namespace Tharga.Team;

public interface ITeam
{
    string Key { get; }
    string Name { get; }
    string Icon { get; }
}

public interface ITeam<TMember> : ITeam
    where TMember : ITeamMember
{
    public TMember[] Members { get; init; }
}
