using Tharga.Blazor.Framework.Localization;
using Tharga.Toolkit;

namespace Tharga.Blazor.Framework;

/// <summary>
/// Configuration options for Tharga.Blazor generic components.
/// </summary>
public record BlazorOptions
{
    /// <summary>
    /// Application title shown in breadcrumbs and page titles.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The language <see cref="DateTimeView"/> and <see cref="TimeSpanView"/> render in. Unset
    /// falls through to <see cref="System.Globalization.CultureInfo.CurrentUICulture"/>, and then
    /// to English when that is a culture the Toolkit has no wording for.
    /// </summary>
    /// <remarks>
    /// This is the application-wide default, so it suits an app that speaks one language. It
    /// cannot vary per user or per tenant: options are resolved as a singleton, so every circuit
    /// reads the same value. A host that chooses language per tenant should register an
    /// <see cref="IBlazorLanguageProvider"/> instead, which is scoped and takes precedence over
    /// this.
    /// <para>
    /// Settable in code, <c>o.Language = Language.Sv</c>, or per environment under the
    /// <c>Tharga:Blazor</c> configuration section.
    /// </para>
    /// </remarks>
    public Language? Language { get; set; }

    /// <summary>
    /// Whether <see cref="CustomErrorBoundary"/> may render exception details — the message and the
    /// stack trace — to the user. Off unless set, so stack traces never reach a production user by
    /// default.
    /// </summary>
    /// <remarks>
    /// The library cannot decide this for itself: it holds no ASP.NET Core hosting reference, and
    /// neither <c>IWebHostEnvironment</c> nor <c>IHostEnvironment</c> is registered on Blazor
    /// WebAssembly. The host always knows, so the host decides — either in code,
    /// <c>o.ShowExceptionDetails = builder.Environment.IsDevelopment()</c>, or per environment under
    /// the <c>Tharga:Blazor</c> configuration section.
    /// <para>
    /// Access denial is never affected. An <see cref="UnauthorizedAccessException"/> renders as a
    /// compact "Access denied" panel with no stack trace regardless of this setting.
    /// </para>
    /// </remarks>
    public bool? ShowExceptionDetails { get; set; }
}
