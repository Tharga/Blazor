namespace Tharga.Blazor.Features.Team;

public abstract record TeamMemberBase : ITeamMember
{
    public required string Key { get; init; }
    public required string EMail { get; init; }
}