namespace Tharga.Team;

public interface IUser
{
    public string Key { get; }
    public string Identity { get; }
    public string EMail { get; }
}

public interface ITeamMember
{
    public string Key { get; }
    public string Name { get; }

    //[Obsolete("This field will be deprecated.")]
    //string EMail { get; }

    public Invitation Invitation { get; }
    public DateTime? LastSeen { get; }
    public MembershipState? State { get; }
    public AccessLevel AccessLevel { get; }
}

//public abstract record TeamMemberBase : ITeamMember
//{
//    public required string Key { get; set; } //TODO: PlutusWave needs to clean database.
//    public required string Name { get; init; }
//    public required Invitation Invitation { get; init; }
//    public required DateTime? LastSeen { get; init; }
//    public required MembershipState? State { get; init; }
//    public required AccessLevel AccessLevel { get; init;  }
//}

//public interface IInvitation
//{
//    public string EMail { get; init; }
//    public string InviteCode { get; init; }
//    public DateTime InviteTime { get; init; }
//}

public record Invitation
{
    public required string EMail { get; init; }
    public required string InviteKey { get; init; }
    public required DateTime InviteTime { get; init; }
}

public enum MembershipState
{
    Member,
    Invited,
    Rejected,
}

public enum AccessLevel
{
    Owner,
    Administrator,
    User,
    Viewer
}
