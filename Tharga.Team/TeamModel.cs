namespace Tharga.Team;

public record TeamModel : ITeam
{
    public required string Key { get; init; }
    public required string Name { get; init; }
    public string Icon { get; init; }
    public ITeamMember[] Members { get; init; }
    //public required bool AllowDeveloperAccess { get; init; } = true;
}