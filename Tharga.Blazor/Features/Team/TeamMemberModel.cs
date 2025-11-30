namespace Tharga.Blazor.Features.Team;

public abstract record TeamMemberBase : ITeamMember
{
    public string Key { get; set; }
    public required string EMail { get; init; }
    public DateTime? LastSeen { get; init; }
}