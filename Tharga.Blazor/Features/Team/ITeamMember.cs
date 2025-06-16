namespace Tharga.Blazor.Features.Team;

public interface ITeamMember
{
    string Key { get; }
    string EMail { get; }
    string Name { get; }
}