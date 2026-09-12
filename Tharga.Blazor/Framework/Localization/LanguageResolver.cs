using System.Globalization;
using Microsoft.Extensions.Options;
using Tharga.Toolkit;

namespace Tharga.Blazor.Framework.Localization;

/// <summary>
/// Decides which language a date or duration component renders in.
/// </summary>
/// <remarks>
/// Resolution runs in this order, taking the first that has an opinion:
/// <list type="number">
/// <item><description>the component's own <c>Language</c> parameter</description></item>
/// <item><description><see cref="IBlazorLanguageProvider.GetLanguage"/> — per site, tenant or user</description></item>
/// <item><description><see cref="BlazorOptions.Language"/> — application-wide</description></item>
/// <item><description><see cref="CultureInfo.CurrentUICulture"/></description></item>
/// <item><description>English</description></item>
/// </list>
/// Resolution never throws. A culture the Toolkit has no wording for — German, or the invariant
/// culture — falls through to English rather than failing the render.
/// </remarks>
public class LanguageResolver
{
    private const Language FallbackLanguage = Language.En;

    private readonly IBlazorLanguageProvider _languageProvider;
    private readonly IOptions<BlazorOptions> _options;

    /// <summary>
    /// Creates a resolver over the host's language provider and application options.
    /// </summary>
    public LanguageResolver(IBlazorLanguageProvider languageProvider, IOptions<BlazorOptions> options)
    {
        _languageProvider = languageProvider;
        _options = options;
    }

    /// <summary>
    /// Resolves the language to render in, preferring <paramref name="language"/> when supplied.
    /// </summary>
    /// <param name="language">The component's explicit language, or <c>null</c> to resolve one.</param>
    public Language Resolve(Language? language = null)
    {
        return language
               ?? _languageProvider?.GetLanguage()
               ?? _options?.Value?.Language
               ?? FromCulture(CultureInfo.CurrentUICulture)
               ?? FallbackLanguage;
    }

    private static Language? FromCulture(CultureInfo culture)
    {
        var name = culture?.TwoLetterISOLanguageName;
        if (string.IsNullOrWhiteSpace(name)) return null;

        return Enum.TryParse<Language>(name, true, out var language) && Enum.IsDefined(language)
            ? language
            : null;
    }
}
