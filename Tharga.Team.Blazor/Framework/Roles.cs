namespace Tharga.Team.Blazor.Framework;

public static class Roles
{
    /// <summary>Role assigned to any authenticated team member regardless of access level.</summary>
    public const string TeamMember = "TeamMember";

    [Obsolete($"Use {nameof(TeamMember)} instead.")]
    public const string TeamUser = TeamMember;

    public const string Developer = "Developer";
}
