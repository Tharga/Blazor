using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tharga.Blazor.Framework;
using Tharga.Blazor.Framework.Localization;
using Tharga.Toolkit;

namespace Tharga.Blazor.Tests;

public class DateTimeViewTests : BunitContext
{
    private const string EnglishMarker = "ago";
    private const string SwedishMarker = "sedan";

    private static readonly DateTime TwoHoursAgo = DateTime.UtcNow.AddHours(-2);

    private readonly FakeLanguageProvider _provider = new();
    private readonly BlazorOptions _options = new();

    public DateTimeViewTests()
    {
        Services.AddSingleton<IBlazorLanguageProvider>(_provider);
        Services.AddSingleton<IOptions<BlazorOptions>>(new OptionsWrapper<BlazorOptions>(_options));
        Services.AddSingleton<LanguageResolver>();
        SetRendererInfo(new RendererInfo("Server", isInteractive: true));
    }

    private IRenderedComponent<DateTimeView> RenderView(Action<ComponentParameterCollectionBuilder<DateTimeView>>? configure = null)
    {
        return Render<DateTimeView>(parameters =>
        {
            parameters.Add(x => x.Date, TwoHoursAgo);
            configure?.Invoke(parameters);
        });
    }

    [Fact]
    public void ByDefault_TheTextIsTheRelativeFormAndTheTooltipIsTheTimestamp()
    {
        var expectedText = TwoHoursAgo.ToLocalDurationString(new DurationOptions
        {
            StringOptions = DurationStringOptionsExtensions.Get(Language.En)
        });
        var expectedTooltip = TwoHoursAgo.ToLocalDateTimeString();
        _options.Language = Language.En;

        var view = RenderView();

        var span = view.Find("span");
        Assert.Equal(expectedText, span.TextContent);
        Assert.Equal(expectedTooltip, span.GetAttribute("title"));
    }

    [Fact]
    public void TheLanguageParameterRendersSwedish()
    {
        var view = RenderView(parameters => parameters.Add(x => x.Language, Language.Sv));

        Assert.Contains(SwedishMarker, view.Markup);
        Assert.DoesNotContain(EnglishMarker, view.Markup);
    }

    [Fact]
    public void AHostLanguageProviderRendersSwedishWithNoMarkupChange()
    {
        _provider.Language = Language.Sv;

        var view = RenderView();

        Assert.Contains(SwedishMarker, view.Markup);
    }

    [Fact]
    public void TheApplicationOptionRendersSwedishWithNoMarkupChange()
    {
        _options.Language = Language.Sv;

        var view = RenderView();

        Assert.Contains(SwedishMarker, view.Markup);
    }

    [Fact]
    public void TheParameterBeatsTheProvider()
    {
        _provider.Language = Language.Sv;

        var view = RenderView(parameters => parameters.Add(x => x.Language, Language.En));

        Assert.Contains(EnglishMarker, view.Markup);
    }

    [Fact]
    public void TheProviderBeatsTheApplicationOption()
    {
        _options.Language = Language.En;
        _provider.Language = Language.Sv;

        var view = RenderView();

        Assert.Contains(SwedishMarker, view.Markup);
    }

    [Fact]
    public void AbsoluteDisplaySwapsTheTextAndTheTooltip()
    {
        _options.Language = Language.En;
        var expectedTooltip = TwoHoursAgo.ToLocalDurationString(new DurationOptions
        {
            StringOptions = DurationStringOptionsExtensions.Get(Language.En)
        });

        var view = RenderView(parameters => parameters.Add(x => x.Display, EDateTimeDisplay.Absolute));

        var span = view.Find("span");
        Assert.Equal(TwoHoursAgo.ToLocalDateTimeString(), span.TextContent);
        Assert.Equal(expectedTooltip, span.GetAttribute("title"));
    }

    [Fact]
    public void ANullDateRendersAnEmptySpan()
    {
        var view = Render<DateTimeView>(parameters => parameters.Add(x => x.Date, (DateTime?)null));

        Assert.Equal(string.Empty, view.Find("span").TextContent);
    }

    [Fact]
    public async Task WithoutARefreshInterval_TheComponentNeverRendersAgain()
    {
        var view = RenderView();
        var initial = view.RenderCount;

        await Task.Delay(TimeSpan.FromSeconds(1.5), Xunit.TestContext.Current.CancellationToken);

        Assert.Equal(initial, view.RenderCount);
    }

    [Fact]
    public void WithARefreshInterval_TheLabelIsRecomputed()
    {
        var view = RenderView(parameters => parameters.Add(x => x.RefreshInterval, TimeSpan.FromSeconds(1)));
        var initial = view.RenderCount;

        view.WaitForState(() => view.RenderCount > initial, TimeSpan.FromSeconds(10));

        Assert.True(view.RenderCount > initial);
    }

    [Fact]
    public async Task ARefreshIntervalIsIgnoredForAnAbsoluteDisplay()
    {
        var view = RenderView(parameters => parameters
            .Add(x => x.Display, EDateTimeDisplay.Absolute)
            .Add(x => x.RefreshInterval, TimeSpan.FromSeconds(1)));
        var initial = view.RenderCount;

        await Task.Delay(TimeSpan.FromSeconds(1.5), Xunit.TestContext.Current.CancellationToken);

        Assert.Equal(initial, view.RenderCount);
    }

    [Fact]
    public async Task DisposingStopsTheTimer()
    {
        var view = RenderView(parameters => parameters.Add(x => x.RefreshInterval, TimeSpan.FromSeconds(1)));
        view.WaitForState(() => view.RenderCount > 1, TimeSpan.FromSeconds(10));

        await view.Instance.DisposeAsync();
        var afterDispose = view.RenderCount;
        await Task.Delay(TimeSpan.FromSeconds(1.5), Xunit.TestContext.Current.CancellationToken);

        Assert.Equal(afterDispose, view.RenderCount);
    }
}

public class DateTimeViewWithoutServicesTests : BunitContext
{
    private static readonly DateTime TwoHoursAgo = DateTime.UtcNow.AddHours(-2);

    [Fact]
    public void WithNoServicesRegistered_TheComponentStillRenders()
    {
        var view = Render<DateTimeView>(parameters => parameters.Add(x => x.Date, TwoHoursAgo));

        var span = view.Find("span");
        Assert.Equal(TwoHoursAgo.ToLocalDateTimeString(), span.GetAttribute("title"));
        Assert.NotEmpty(span.TextContent);
    }

    [Fact]
    public void WithNoServicesRegistered_TheLanguageParameterStillWins()
    {
        var view = Render<DateTimeView>(parameters => parameters
            .Add(x => x.Date, TwoHoursAgo)
            .Add(x => x.Language, Language.Sv));

        Assert.Contains("sedan", view.Markup);
    }
}
