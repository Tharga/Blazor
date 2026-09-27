using Tharga.Blazor.Features.BreadCrumbs;

namespace Tharga.Blazor.Tests;

public class BreadCrumbServiceTests
{
    [Fact]
    public void AddVirtualSegment_AppendsToEnd()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);

        svc.AddVirtualSegment("search");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.Equal("Search", items.Last().Text);
    }

    [Fact]
    public void AddVirtualSegment_LastVirtualSegmentHasNoLink()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);

        svc.AddVirtualSegment("search");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.Null(items.Last().Path);
    }

    [Fact]
    public void AddVirtualSegment_FiresChangeEvent()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);
        var fired = false;
        svc.ChangeEvent += (_, _) => fired = true;

        svc.AddVirtualSegment("search");

        Assert.True(fired);
    }

    [Fact]
    public void RemoveVirtualSegments_ClearsVirtualSegments()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);
        svc.AddVirtualSegment("search");

        svc.RemoveVirtualSegments();

        var items = svc.BreadCrumbItems.ToArray();
        Assert.DoesNotContain(items, x => x.Text == "Search");
    }

    [Fact]
    public void RemoveVirtualSegments_FiresChangeEvent()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);
        svc.AddVirtualSegment("search");
        var fired = false;
        svc.ChangeEvent += (_, _) => fired = true;

        svc.RemoveVirtualSegments();

        Assert.True(fired);
    }

    [Fact]
    public void Navigation_ClearsVirtualSegmentsWhenPathChanges()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);
        svc.AddVirtualSegment("search");

        nav.ChangeUri("https://localhost/developer/other");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.DoesNotContain(items, x => x.Text == "Search");
    }

    [Fact]
    public void Navigation_PreservesVirtualSegmentsWhenOnlyQueryParamsChange()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);
        svc.AddVirtualSegment("search");

        nav.ChangeUri("https://localhost/developer/log?tab=summary");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.Contains(items, x => x.Text == "Search");
    }

    [Fact]
    public void RelinkSegment_ReplacesPathWithCustomUrl()
    {
        var nav = new FakeNavigationManager("https://localhost/monitor/log/summary/abc123");
        var svc = new BreadCrumbService(nav);

        svc.RelinkSegment("log", "/developer/log?tab=summary");

        var items = svc.BreadCrumbItems.ToArray();
        var logItem = items.FirstOrDefault(x => x.Text == "Log");
        Assert.NotNull(logItem);
        Assert.Equal("/developer/log?tab=summary", logItem.Path);
    }

    [Fact]
    public void RelinkSegment_FiresChangeEvent()
    {
        var nav = new FakeNavigationManager("https://localhost/monitor/log/summary/abc123");
        var svc = new BreadCrumbService(nav);
        var fired = false;
        svc.ChangeEvent += (_, _) => fired = true;

        svc.RelinkSegment("log", "/developer/log?tab=summary");

        Assert.True(fired);
    }

    [Fact]
    public void RelinkSegment_DoesNotAffectOtherSegments()
    {
        var nav = new FakeNavigationManager("https://localhost/monitor/log/summary/abc123");
        var svc = new BreadCrumbService(nav);

        svc.RelinkSegment("log", "/developer/log?tab=summary");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.Contains(items, x => x.Text == "Monitor");
        Assert.Contains(items, x => x.Text == "Summary");
    }

    [Fact]
    public void UnlinkSegment_PersistsAfterQueryParamChange()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);
        svc.UnlinkSegment("log");

        nav.ChangeUri("https://localhost/developer/log?tab=summary");

        var items = svc.BreadCrumbItems.ToArray();
        var logItem = items.FirstOrDefault(x => x.Text == "Log");
        Assert.NotNull(logItem);
        Assert.Null(logItem.Path);
    }

    [Fact]
    public void Modifiers_DifferingOnlyInCase_DoNotThrow()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc/records/def");
        var svc = new BreadCrumbService(nav);
        svc.RemoveSegment("records");

        svc.UnlinkSegment("Records");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.DoesNotContain(items, x => x.Text == "Records");
    }

    [Fact]
    public void RemoveVirtualSegments_DoesNotFireEventWhenAlreadyEmpty()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);
        var fired = false;
        svc.ChangeEvent += (_, _) => fired = true;

        svc.RemoveVirtualSegments();

        Assert.False(fired);
    }

    [Fact]
    public void RegisterVirtualSegmentQueryParam_ShowsQueryParamValueAsVirtualSegment()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log?tab=summary");
        var svc = new BreadCrumbService(nav);

        svc.RegisterVirtualSegmentQueryParam("tab");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.Contains(items, x => x.Text == "Summary");
    }

    [Fact]
    public void RegisterVirtualSegmentQueryParam_UpdatesOnNavigation()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log?tab=search");
        var svc = new BreadCrumbService(nav);
        svc.RegisterVirtualSegmentQueryParam("tab");

        nav.ChangeUri("https://localhost/developer/log?tab=summary");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.Contains(items, x => x.Text == "Summary");
        Assert.DoesNotContain(items, x => x.Text == "Search");
    }

    [Fact]
    public void RegisterVirtualSegmentQueryParam_ClearsWhenNavigatingToPathWithoutParam()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log?tab=summary");
        var svc = new BreadCrumbService(nav);
        svc.RegisterVirtualSegmentQueryParam("tab");

        nav.ChangeUri("https://localhost/developer/other");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.DoesNotContain(items, x => x.Text == "Summary");
    }

    [Fact]
    public void RegisterVirtualSegmentQueryParam_FiresChangeEvent()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log?tab=summary");
        var svc = new BreadCrumbService(nav);
        var fired = false;
        svc.ChangeEvent += (_, _) => fired = true;

        svc.RegisterVirtualSegmentQueryParam("tab");

        Assert.True(fired);
    }

    [Fact]
    public void AddVirtualSegment_WithPath_CreatesLinkableSegment()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log/detail/abc123");
        var svc = new BreadCrumbService(nav);

        svc.AddVirtualSegment("search", "/developer/log?tab=search");
        svc.AddVirtualSegment("abc123");

        var items = svc.BreadCrumbItems.ToArray();
        var searchItem = items.FirstOrDefault(x => x.Text == "Search");
        Assert.NotNull(searchItem);
        Assert.Equal("/developer/log?tab=search", searchItem.Path);
    }

    [Fact]
    public void AddVirtualSegment_WithPath_LastSegmentStillHasNoLink()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log/detail/abc123");
        var svc = new BreadCrumbService(nav);

        svc.AddVirtualSegment("search", "/developer/log?tab=search");
        svc.AddVirtualSegment("abc123");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.Null(items.Last().Path);
        Assert.Equal("/developer/log?tab=search", items[^2].Path);
    }

    [Fact]
    public void SetSegmentText_ChangesTextAndKeepsPositionAndLink()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/7318ed96/records/6ab8cfeb");
        var svc = new BreadCrumbService(nav);

        svc.SetSegmentText("7318ed96", "KS 2026-14");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.Equal(["Cases", "KS 2026-14", "Records", "6ab8cfeb"], items.Select(x => x.Text));
        Assert.Equal("cases/7318ed96", items[1].Path);
    }

    [Fact]
    public void SetSegmentText_ShowsTextAsGiven()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc");
        var svc = new BreadCrumbService(nav);

        svc.SetSegmentText("cases", "ärenden");

        Assert.Equal("ärenden", svc.BreadCrumbItems.First().Text);
    }

    [Fact]
    public void SetSegmentText_CombinesWithUnlink()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc/records/def");
        var svc = new BreadCrumbService(nav);

        svc.UnlinkSegment("records");
        svc.SetSegmentText("records", "Handlingar");

        var item = svc.BreadCrumbItems.ToArray()[2];
        Assert.Equal("Handlingar", item.Text);
        Assert.Null(item.Path);
    }

    [Fact]
    public void SetSegmentText_CombinesWithRelink()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc/records/def");
        var svc = new BreadCrumbService(nav);

        svc.SetSegmentText("records", "Handlingar");
        svc.RelinkSegment("records", "/records");

        var item = svc.BreadCrumbItems.ToArray()[2];
        Assert.Equal("Handlingar", item.Text);
        Assert.Equal("/records", item.Path);
    }

    [Fact]
    public void SetSegmentText_LastCallWins()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc");
        var svc = new BreadCrumbService(nav);

        svc.SetSegmentText("abc", "First");
        svc.SetSegmentText("ABC", "Second");

        Assert.Equal("Second", svc.BreadCrumbItems.Last().Text);
    }

    [Fact]
    public void SetSegmentText_Null_RestoresDefaultText()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc");
        var svc = new BreadCrumbService(nav);
        svc.SetSegmentText("cases", "Ärenden");

        svc.SetSegmentText("cases", null);

        Assert.Equal("Cases", svc.BreadCrumbItems.First().Text);
    }

    [Fact]
    public void SetSegmentText_OnlyAppliesToCurrentUrl()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc");
        var svc = new BreadCrumbService(nav);
        svc.SetSegmentText("cases", "Ärenden");

        nav.ChangeUri("https://localhost/cases/other");

        Assert.Equal("Cases", svc.BreadCrumbItems.First().Text);
    }

    [Fact]
    public void SetSegmentText_FiresChangeEventOnlyWhenTextChanges()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc");
        var svc = new BreadCrumbService(nav);
        svc.SetSegmentText("cases", "Ärenden");
        var fired = 0;
        svc.ChangeEvent += (_, _) => fired++;

        svc.SetSegmentText("cases", "Ärenden");
        svc.SetSegmentText("cases", "Mål");

        Assert.Equal(1, fired);
    }

    [Fact]
    public void RemoveSegment_AfterSetSegmentText_StillRemoves()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc/records/def");
        var svc = new BreadCrumbService(nav);
        svc.SetSegmentText("records", "Handlingar");

        svc.RemoveSegment("records");

        Assert.DoesNotContain(svc.BreadCrumbItems, x => x.Text == "Handlingar");
    }

    [Fact]
    public void TextProvider_TranslatesUrlSegments()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc");
        var svc = new BreadCrumbService(nav, new FakeTextProvider { ["cases"] = "Ärenden" });

        Assert.Equal(["Ärenden", "Abc"], svc.BreadCrumbItems.Select(x => x.Text));
    }

    [Fact]
    public void TextProvider_ReceivesSegmentAndRoutePath()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc");
        var provider = new FakeTextProvider();
        var svc = new BreadCrumbService(nav, provider);

        _ = svc.BreadCrumbItems.ToArray();

        Assert.Equal([("cases", "cases"), ("abc", "cases/abc")], provider.Calls);
    }

    [Fact]
    public void TextProvider_IsNotAskedForVirtualSegments()
    {
        var nav = new FakeNavigationManager("https://localhost/cases");
        var provider = new FakeTextProvider { ["search"] = "Sök" };
        var svc = new BreadCrumbService(nav, provider);

        svc.AddVirtualSegment("search");

        Assert.Equal("Search", svc.BreadCrumbItems.Last().Text);
    }

    [Fact]
    public void SetSegmentText_WinsOverTextProvider()
    {
        var nav = new FakeNavigationManager("https://localhost/cases/abc");
        var svc = new BreadCrumbService(nav, new FakeTextProvider { ["abc"] = "Translated" });

        svc.SetSegmentText("abc", "KS 2026-14");

        Assert.Equal("KS 2026-14", svc.BreadCrumbItems.Last().Text);
    }

    [Fact]
    public void TextProvider_IsAskedOnEveryRead()
    {
        var nav = new FakeNavigationManager("https://localhost/cases");
        var provider = new FakeTextProvider { ["cases"] = "Cases" };
        var svc = new BreadCrumbService(nav, provider);
        Assert.Equal("Cases", svc.BreadCrumbItems.Single().Text);

        provider["cases"] = "Ärenden";

        Assert.Equal("Ärenden", svc.BreadCrumbItems.Single().Text);
    }

    [Fact]
    public void Refresh_FiresChangeEvent()
    {
        var nav = new FakeNavigationManager("https://localhost/cases");
        var svc = new BreadCrumbService(nav);
        var fired = false;
        svc.ChangeEvent += (_, _) => fired = true;

        svc.Refresh();

        Assert.True(fired);
    }

    private class FakeTextProvider : IBreadCrumbTextProvider
    {
        private readonly Dictionary<string, string> _texts = new();

        public List<(string Segment, string Path)> Calls { get; } = [];

        public string this[string segment]
        {
            set => _texts[segment] = value;
        }

        public string? GetText(string segment, string path)
        {
            Calls.Add((segment, path));
            return _texts.GetValueOrDefault(segment);
        }
    }
}
