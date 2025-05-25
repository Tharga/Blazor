namespace Tharga.Blazor.Features.Team;

public interface ITeamMember
{
    string Key { get; }
    string EMail { get; }
    string Name { get; }
}

public interface ITeam
{
    string Key { get; }
    string Icon { get; }
    string Name { get; }
}