using Blazored.LocalStorage;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Tharga.Blazor.Tests;

public class ExpandableCardTests : BunitContext
{
    private const string EnglishTitle = "Secrecy classes";
    private const string SwedishTitle = "Sekretessklasser";
    private const string EnglishTitleStorageKey = "ExpandableCard.Secrecy classes.Selected";
    private const string SwedishTitleStorageKey = "ExpandableCard.Sekretessklasser.Selected";
    private const string ExplicitKey = "caseManagement.secrecyClasses";
    private const string ExplicitStorageKey = "ExpandableCard.caseManagement.secrecyClasses.Selected";
    private const string BodyMarker = "expandable-card-body";
    private const string TitleBarSelector = "div[style*=\"cursor: pointer\"]";

    private readonly FakeLocalStorageService _storage = new();

    public ExpandableCardTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton<ILocalStorageService>(_storage);
        SetRendererInfo(new RendererInfo("Server", isInteractive: true));
    }

    private IRenderedComponent<ExpandableCard> RenderCard(Action<ComponentParameterCollectionBuilder<ExpandableCard>>? configure = null)
    {
        var card = Render<ExpandableCard>(parameters =>
        {
            parameters.Add(x => x.Text, EnglishTitle);
            parameters.AddChildContent($"<p>{BodyMarker}</p>");
            configure?.Invoke(parameters);
        });

        card.WaitForState(() => card.RenderCount > 0);

        return card;
    }

    private static void RenderWithTitle(IRenderedComponent<ExpandableCard> card, string title)
    {
        card.Render(parameters =>
        {
            parameters.Add(x => x.Text, title);
            parameters.AddChildContent($"<p>{BodyMarker}</p>");
        });
    }

    private static bool IsExpanded(IRenderedComponent<ExpandableCard> card) => card.Markup.Contains(BodyMarker);

    private static void ClickTitleBar(IRenderedComponent<ExpandableCard> card) => card.Find(TitleBarSelector).Click();

    [Fact]
    public void StoredExpandedState_IsRestored()
    {
        _storage.Seed(ExplicitStorageKey, true);

        var card = RenderCard(p => p.Add(x => x.StateKey, ExplicitKey));

        Assert.True(IsExpanded(card));
        Assert.Equal(ExplicitStorageKey, Assert.Single(_storage.Reads));
    }

    [Fact]
    public void StoredCollapsedState_OverridesTheSelectedDefault()
    {
        _storage.Seed(ExplicitStorageKey, false);

        var card = RenderCard(p =>
        {
            p.Add(x => x.StateKey, ExplicitKey);
            p.Add(x => x.Selected, true);
        });

        Assert.False(IsExpanded(card));
    }

    [Fact]
    public void StoredState_IsAppliedWithoutASecondRender()
    {
        _storage.Seed(ExplicitStorageKey, true);

        var card = RenderCard(p => p.Add(x => x.StateKey, ExplicitKey));

        Assert.True(IsExpanded(card));
        Assert.Equal(1, card.RenderCount);
    }

    [Fact]
    public void NothingStored_FallsBackToTheSelectedDefault()
    {
        var card = RenderCard(p =>
        {
            p.Add(x => x.StateKey, ExplicitKey);
            p.Add(x => x.Selected, true);
        });

        Assert.True(IsExpanded(card));
        Assert.Equal(1, card.RenderCount);
    }

    [Fact]
    public void Toggling_PersistsTheNewStateUnderTheNamespacedKey()
    {
        var card = RenderCard(p => p.Add(x => x.StateKey, ExplicitKey));

        ClickTitleBar(card);

        card.WaitForAssertion(() => Assert.Equal(ExplicitStorageKey, Assert.Single(_storage.Writes)));
        Assert.True(_storage.Read<bool?>(ExplicitStorageKey));
    }

    [Fact]
    public void TogglingTwice_PersistsTheLatestState()
    {
        var card = RenderCard(p => p.Add(x => x.StateKey, ExplicitKey));

        ClickTitleBar(card);
        ClickTitleBar(card);

        card.WaitForAssertion(() => Assert.Equal(2, _storage.Writes.Count));
        Assert.False(IsExpanded(card));
        Assert.False(_storage.Read<bool?>(ExplicitStorageKey));
    }

    [Fact]
    public void WithoutStateKey_TheKeyFallsBackToText()
    {
        var card = RenderCard();

        ClickTitleBar(card);

        card.WaitForAssertion(() => Assert.Equal(EnglishTitleStorageKey, Assert.Single(_storage.Writes)));
    }

    [Fact]
    public void StateKey_TakesPrecedenceOverText()
    {
        var card = RenderCard(p => p.Add(x => x.StateKey, ExplicitKey));

        ClickTitleBar(card);

        card.WaitForAssertion(() => Assert.Equal(ExplicitStorageKey, Assert.Single(_storage.Writes)));
        Assert.DoesNotContain(EnglishTitleStorageKey, _storage.Keys);
    }

    [Fact]
    public void TextArrivingAfterTheFirstRender_DoesNotMoveTheKey()
    {
        var card = RenderCard();

        RenderWithTitle(card, SwedishTitle);
        ClickTitleBar(card);

        card.WaitForAssertion(() => Assert.Equal(EnglishTitleStorageKey, Assert.Single(_storage.Writes)));
        Assert.DoesNotContain(SwedishTitleStorageKey, _storage.Keys);
    }

    [Fact]
    public void TextArrivingAfterTheFirstRender_RestoresOnTheNextLoad()
    {
        var first = RenderCard();
        RenderWithTitle(first, SwedishTitle);
        ClickTitleBar(first);
        first.WaitForAssertion(() => Assert.Single(_storage.Writes));

        var second = RenderCard();

        Assert.True(IsExpanded(second));
    }

    [Fact]
    public void SameTextWithDifferentStateKeys_DoNotShareState()
    {
        var first = RenderCard(p => p.Add(x => x.StateKey, "pageOne.card"));
        var second = RenderCard(p => p.Add(x => x.StateKey, "pageTwo.card"));

        ClickTitleBar(first);

        first.WaitForAssertion(() => Assert.Equal("ExpandableCard.pageOne.card.Selected", Assert.Single(_storage.Writes)));
        Assert.True(IsExpanded(first));
        Assert.False(IsExpanded(second));
    }

    [Fact]
    public void AllowSaveStateFalse_NeitherReadsNorWritesStorage()
    {
        _storage.Seed(ExplicitStorageKey, true);

        var card = RenderCard(p =>
        {
            p.Add(x => x.StateKey, ExplicitKey);
            p.Add(x => x.AllowSaveState, false);
        });
        ClickTitleBar(card);

        Assert.Empty(_storage.Reads);
        Assert.Empty(_storage.Writes);
        Assert.True(IsExpanded(card));
    }

    [Fact]
    public void WithoutTextOrStateKey_StorageIsNotTouched()
    {
        var card = Render<ExpandableCard>(p => p.AddChildContent($"<p>{BodyMarker}</p>"));

        ClickTitleBar(card);

        Assert.Empty(_storage.Reads);
        Assert.Empty(_storage.Writes);
        Assert.True(IsExpanded(card));
    }

    [Fact]
    public void NonInteractiveRenderer_DoesNotTouchStorage()
    {
        _storage.Seed(ExplicitStorageKey, true);
        SetRendererInfo(new RendererInfo("Static", isInteractive: false));

        var card = RenderCard(p => p.Add(x => x.StateKey, ExplicitKey));

        Assert.Empty(_storage.Reads);
        Assert.False(IsExpanded(card));
        Assert.Equal(1, card.RenderCount);
    }
}

public class ExpandableCardUnavailableStorageTests : BunitContext
{
    private const string BodyMarker = "expandable-card-body";
    private const string TitleBarSelector = "div[style*=\"cursor: pointer\"]";
    private const string StorageKey = "ExpandableCard.Secrecy classes.Selected";

    private readonly FakeLocalStorageService _storage = new() { Failure = new JSException("localStorage is not available") };

    public ExpandableCardUnavailableStorageTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton<ILocalStorageService>(_storage);
        SetRendererInfo(new RendererInfo("Server", isInteractive: true));
    }

    [Fact]
    public void UnavailableStorage_StillRendersAndTogglesTheCard()
    {
        var card = Render<ExpandableCard>(parameters =>
        {
            parameters.Add(x => x.Text, "Secrecy classes");
            parameters.Add(x => x.Selected, true);
            parameters.AddChildContent($"<p>{BodyMarker}</p>");
        });
        card.WaitForState(() => card.RenderCount > 0);

        Assert.Contains(BodyMarker, card.Markup);
        Assert.Equal(StorageKey, Assert.Single(_storage.Reads));

        card.Find(TitleBarSelector).Click();

        Assert.DoesNotContain(BodyMarker, card.Markup);
        card.WaitForAssertion(() => Assert.Equal(StorageKey, Assert.Single(_storage.Writes)));
    }
}
