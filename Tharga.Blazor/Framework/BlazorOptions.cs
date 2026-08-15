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
