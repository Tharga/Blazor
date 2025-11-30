namespace Tharga.Blazor.Features.Team;

public abstract record TeamMemberBase : ITeamMember
{
    public required string Key { get; set; }
    public required string EMail { get; init; }
    public required DateTime? LastSeen { get; init; }
}