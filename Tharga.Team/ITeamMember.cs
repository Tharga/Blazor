namespace Tharga.Team;

public interface ITeamMember
{
    public string Key { get; }
    public string Name { get; }
    public Invitation Invitation { get; }
    public DateTime? LastSeen { get; }
    public MembershipState? State { get; }
    public AccessLevel AccessLevel { get; }
}