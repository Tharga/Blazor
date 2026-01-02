namespace Tharga.Team;

public record InviteUserModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public AccessLevel AccessLevel { get; set; }
}