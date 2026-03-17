namespace Tharga.Team;

/// <summary>
/// Declares the minimum access level required to call this method.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequireAccessLevelAttribute : Attribute
{
    public AccessLevel MinimumLevel { get; }

    public RequireAccessLevelAttribute(AccessLevel minimumLevel)
    {
        MinimumLevel = minimumLevel;
    }
}
