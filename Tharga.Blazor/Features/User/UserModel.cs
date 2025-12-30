namespace Tharga.Blazor.Features.User;

public record UserModel
{
    public required string Key { get; set; }
    public required string EMail { get; set; }
    public required DateTime? LastSeen { get; set; }
    //public required ITeam[] Teams { get; set; }
}