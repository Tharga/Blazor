using Tharga.Team.Blazor.Features.Authentication;
using Tharga.Team.Service;
using Tharga.Team.Service.Audit;

namespace Tharga.Team.Blazor.Framework;

/// <summary>
/// Top-level options for configuring the Tharga Platform via a single AddThargaPlatform call.
/// All sub-options have sensible defaults. Configure only what you need.
/// </summary>
public class ThargaPlatformOptions
{
    /// <summary>
    /// Options for the Blazor UI layer (title, team service types, auth state decoration).
    /// </summary>
    public ThargaBlazorOptions Blazor { get; } = new();

    /// <summary>
    /// Options for Azure AD / OIDC authentication.
    /// </summary>
    public ThargaAuthOptions Auth { get; } = new();

    /// <summary>
    /// Options for API key authentication scheme.
    /// Set to null to skip API key authentication registration.
    /// </summary>
    public ApiKeyOptions ApiKey { get; set; } = new();

    /// <summary>
    /// Options for MVC controllers and Swagger. Set to null to skip.
    /// </summary>
    public ThargaControllerOptions Controllers { get; set; } = new();

    /// <summary>
    /// Configure scopes for access-level-based authorization.
    /// When null, scope registration is skipped and scope UI columns are hidden.
    /// </summary>
    public Action<ScopeRegistry> ConfigureScopes { get; set; }

    /// <summary>
    /// Configure tenant roles for role-based authorization within teams.
    /// When null, tenant role registration is skipped and role UI columns are hidden.
    /// </summary>
    public Action<TenantRoleRegistry> ConfigureTenantRoles { get; set; }

    /// <summary>
    /// Options for audit logging. Set to null to skip audit registration.
    /// </summary>
    public AuditOptions Audit { get; set; }
}
