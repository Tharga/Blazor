namespace Tharga.Blazor.Features.Authentication;

/// <summary>
/// Options for configuring Tharga authentication registration.
/// </summary>
public class ThargaAuthOptions
{
    /// <summary>
    /// Path for the login endpoint. Defaults to "/login".
    /// </summary>
    public string LoginPath { get; set; } = "/login";

    /// <summary>
    /// Path for the logout endpoint. Defaults to "/logout".
    /// </summary>
    public string LogoutPath { get; set; } = "/logout";

    /// <summary>
    /// When true, validates that the AzureAd configuration section exists at startup.
    /// Defaults to true.
    /// </summary>
    public bool ValidateConfiguration { get; set; } = true;
}
