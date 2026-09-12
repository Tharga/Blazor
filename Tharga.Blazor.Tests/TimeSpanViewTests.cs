using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tharga.Blazor.Framework;
using Tharga.Blazor.Framework.Localization;
using Tharga.Toolkit;

namespace Tharga.Blazor.Tests;

public class TimeSpanViewTests : BunitContext
{
    private const string EnglishMinutes = "5 minutes";
    private const string SwedishMinutes = "5 minuter";

    private static readonly TimeSpan FiveMinutes = TimeSpan.FromMinutes(5);

    private readonly FakeLanguageProvider _provider = new();
    private readonly BlazorOptions _options = new();

    public TimeSpanViewTests()
    {
        Services.AddSingleton<IBlazorLanguageProvider>(_provider);
        Services.AddSingleton<IOptions<BlazorOptions>>(new OptionsWrapper<BlazorOptions>(_options));
        Services.AddSingleton<LanguageResolver>();
    }

    private IRenderedComponent<TimeSpanView> RenderView(Action<ComponentParameterCollectionBuilder<TimeSpanView>>? configure = null)
    {
        return Render<TimeSpanView>(parameters =>
        {
            parameters.Add(x => x.TimeSpan, FiveMinutes);
            configure?.Invoke(parameters);
        });
    }

    [Fact]
    public void ByDefault_TheTextIsEnglishAndTheTooltipIsTheRawValue()
    {
        _options.Language = Language.En;

        var view = RenderView();

        var span = view.Find("span");
        Assert.Equal(EnglishMinutes, span.TextContent);
        Assert.Equal(FiveMinutes.ToString(), span.GetAttribute("title"));
    }

    [Fact]
    public void TheLanguageParameterRendersSwedish()
    {
        var view = RenderView(parameters => parameters.Add(x => x.Language, Language.Sv));

        Assert.Equal(SwedishMinutes, view.Find("span").TextContent);
    }

    [Fact]
    public void AHostLanguageProviderRendersSwedishWithNoMarkupChange()
    {
        _provider.Language = Language.Sv;

        var view = RenderView();

        Assert.Equal(SwedishMinutes, view.Find("span").TextContent);
    }

    [Fact]
    public void TheApplicationOptionRendersSwedishWithNoMarkupChange()
    {
        _options.Language = Language.Sv;

        var view = RenderView();

        Assert.Equal(SwedishMinutes, view.Find("span").TextContent);
    }

    [Fact]
    public void TheParameterBeatsTheProvider()
    {
        _provider.Language = Language.Sv;

        var view = RenderView(parameters => parameters.Add(x => x.Language, Language.En));

        Assert.Equal(EnglishMinutes, view.Find("span").TextContent);
    }

    [Fact]
    public void ANullTimeSpanRendersAnEmptySpan()
    {
        var view = Render<TimeSpanView>(parameters => parameters.Add(x => x.TimeSpan, (TimeSpan?)null));

        Assert.Equal(string.Empty, view.Find("span").TextContent);
    }
}

public class TimeSpanViewWithoutServicesTests : BunitContext
{
    [Fact]
    public void WithNoServicesRegistered_TheComponentStillRenders()
    {
        var view = Render<TimeSpanView>(parameters => parameters.Add(x => x.TimeSpan, TimeSpan.FromMinutes(5)));

        Assert.NotEmpty(view.Find("span").TextContent);
    }

    [Fact]
    public void WithNoServicesRegistered_TheLanguageParameterStillWins()
    {
        var view = Render<TimeSpanView>(parameters => parameters
            .Add(x => x.TimeSpan, TimeSpan.FromMinutes(5))
            .Add(x => x.Language, Language.Sv));

        Assert.Equal("5 minuter", view.Find("span").TextContent);
    }
}
