namespace Tharga.Team;

public interface ITeamMember
{
    string Key { get; }
    string EMail { get; }
    DateTime? LastSeen { get; }
}