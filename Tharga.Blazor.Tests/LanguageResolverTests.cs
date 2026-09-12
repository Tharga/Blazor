using System.Globalization;
using Microsoft.Extensions.Options;
using Tharga.Blazor.Framework;
using Tharga.Blazor.Framework.Localization;
using Tharga.Toolkit;

namespace Tharga.Blazor.Tests;

public class LanguageResolverTests
{
    private readonly FakeLanguageProvider _provider = new();
    private readonly BlazorOptions _options = new();

    private LanguageResolver Resolver() => new(_provider, new OptionsWrapper<BlazorOptions>(_options));

    private static Language Under(string? culture, Func<Language> resolve)
    {
        var original = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentUICulture = culture == null ? CultureInfo.InvariantCulture : new CultureInfo(culture);
            return resolve();
        }
        finally
        {
            CultureInfo.CurrentUICulture = original;
        }
    }

    [Fact]
    public void NothingSet_FallsBackToEnglish()
    {
        Assert.Equal(Language.En, Under("de-DE", () => Resolver().Resolve()));
    }

    [Fact]
    public void CultureIsUsedWhenNothingElseHasAnOpinion()
    {
        Assert.Equal(Language.Sv, Under("sv-SE", () => Resolver().Resolve()));
    }

    [Fact]
    public void OptionsBeatCulture()
    {
        _options.Language = Language.En;

        Assert.Equal(Language.En, Under("sv-SE", () => Resolver().Resolve()));
    }

    [Fact]
    public void ProviderBeatsOptions()
    {
        _options.Language = Language.En;
        _provider.Language = Language.Sv;

        Assert.Equal(Language.Sv, Under("en-US", () => Resolver().Resolve()));
    }

    [Fact]
    public void ParameterBeatsProvider()
    {
        _provider.Language = Language.Sv;

        Assert.Equal(Language.En, Under("sv-SE", () => Resolver().Resolve(Language.En)));
    }

    [Fact]
    public void AProviderWithNoOpinionFallsThrough()
    {
        _provider.Language = null;
        _options.Language = Language.Sv;

        Assert.Equal(Language.Sv, Under("en-US", () => Resolver().Resolve()));
    }

    [Theory]
    [InlineData("sv-SE", Language.Sv)]
    [InlineData("sv", Language.Sv)]
    [InlineData("SV", Language.Sv)]
    [InlineData("en-US", Language.En)]
    [InlineData("en-GB", Language.En)]
    [InlineData("de-DE", Language.En)]
    [InlineData("nb-NO", Language.En)]
    [InlineData("zh-Hans-CN", Language.En)]
    [InlineData("", Language.En)]
    public void CultureResolvesWithoutThrowing(string culture, Language expected)
    {
        Assert.Equal(expected, Under(culture, () => Resolver().Resolve()));
    }

    [Fact]
    public void TheInvariantCultureFallsBackToEnglish()
    {
        Assert.Equal(Language.En, Under(null, () => Resolver().Resolve()));
    }

    [Fact]
    public void AResolverWithNoServicesAtAllStillResolves()
    {
        var resolver = new LanguageResolver(null!, null!);

        Assert.Equal(Language.Sv, Under("sv-SE", () => resolver.Resolve()));
        Assert.Equal(Language.En, Under("de-DE", () => resolver.Resolve()));
        Assert.Equal(Language.Sv, Under("de-DE", () => resolver.Resolve(Language.Sv)));
    }
}
