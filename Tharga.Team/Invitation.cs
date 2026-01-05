namespace Tharga.Team;

public record Invitation
{
    public required string EMail { get; init; }
    public required string InviteKey { get; init; }
    public required DateTime InviteTime { get; init; }
}