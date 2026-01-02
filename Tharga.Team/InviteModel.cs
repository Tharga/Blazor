namespace Tharga.Team;

public record InviteModel
{
    public required string TeamKey { get; init; }
    public required string Code { get; init; }
}