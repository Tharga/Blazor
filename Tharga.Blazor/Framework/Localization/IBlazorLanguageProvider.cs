using Tharga.Toolkit;

namespace Tharga.Blazor.Framework.Localization;

/// <summary>
/// Supplies the language that date and duration components render in, for a host that chooses
/// language per site, per tenant or per signed-in user rather than through
/// <see cref="System.Globalization.CultureInfo.CurrentUICulture"/>.
/// </summary>
/// <remarks>
/// Registered scoped, so in Blazor Server one instance serves one circuit and may read whatever
/// tenant or user context the host already resolves there. The default implementation has no
/// opinion; register your own to override it:
/// <code>
/// services.AddScoped&lt;IBlazorLanguageProvider, TenantLanguageProvider&gt;();
/// </code>
/// Because this is a service rather than a cascading value, it reaches components rendered
/// outside the layout's render tree — a component inside a dialog opened through
/// <c>DialogService</c> resolves the same language as one on the page behind it.
/// </remarks>
public interface IBlazorLanguageProvider
{
    /// <summary>
    /// The language to render in, or <c>null</c> to defer to the application-wide
    /// <see cref="BlazorOptions.Language"/>, then to the current UI culture, then to English.
    /// </summary>
    Language? GetLanguage();
}
