namespace Tharga.Blazor.Features.Team;

public record TeamModel : ITeam
{
    public required string Key { get; init; }
    public required string Name { get; init; }
    public required string Icon { get; init; }
    //public required bool AllowDeveloperAccess { get; init; } = true;
}