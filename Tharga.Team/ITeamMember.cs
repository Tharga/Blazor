namespace Tharga.Team;

public interface ITeamMember
{
    string Key { get; }
    string Name { get; }

    //[Obsolete("This field will be deprecated.")]
    //string EMail { get; }

    public Invitation Invitation { get; }
    DateTime? LastSeen { get; }
    MembershipState? State { get; }
    AccessLevel AccessLevel { get; }
}

//public interface IInvitation
//{
//    public string EMail { get; init; }
//    public string InviteCode { get; init; }
//    public DateTime InviteTime { get; init; }
//}

public record Invitation //: IInvitation
{
    public required string EMail { get; init; }
    public required string InviteCode { get; init; }
    public required DateTime InviteTime { get; init; }
}

public enum MembershipState
{
    Member,
    Invited,
    Evicted,
    Rejected,
}

public enum AccessLevel
{
    Owner,
    Administrator,
    User,
    Viewer
}
