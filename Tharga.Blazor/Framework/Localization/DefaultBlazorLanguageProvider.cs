using Tharga.Toolkit;

namespace Tharga.Blazor.Framework.Localization;

/// <summary>
/// The <see cref="IBlazorLanguageProvider"/> used when a host registers none. It has no opinion,
/// so resolution falls through to <see cref="BlazorOptions.Language"/> and then to the current
/// UI culture.
/// </summary>
internal class DefaultBlazorLanguageProvider : IBlazorLanguageProvider
{
    public Language? GetLanguage() => null;
}
