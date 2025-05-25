namespace Tharga.Blazor.Features.Team;

public record TeamMemberModel : ITeamMember
{
    public required string Key { get; init; }
    public required string EMail { get; init; }
    public string Name { get; init; }
    public required TeamMemberRole Role { get; init; }
}