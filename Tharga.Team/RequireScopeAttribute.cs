namespace Tharga.Team;

/// <summary>
/// Declares the scope required to call this method.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequireScopeAttribute : Attribute
{
    public string Scope { get; }

    public RequireScopeAttribute(string scope)
    {
        Scope = scope;
    }
}
