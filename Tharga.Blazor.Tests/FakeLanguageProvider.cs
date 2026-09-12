using Tharga.Blazor.Framework.Localization;
using Tharga.Toolkit;

namespace Tharga.Blazor.Tests;

internal class FakeLanguageProvider : IBlazorLanguageProvider
{
    public Language? Language { get; set; }

    public Language? GetLanguage() => Language;
}
