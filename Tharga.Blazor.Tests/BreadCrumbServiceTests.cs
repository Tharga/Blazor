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
    public void Navigation_ClearsVirtualSegments()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);
        svc.AddVirtualSegment("search");

        nav.ChangeUri("https://localhost/developer/log");

        var items = svc.BreadCrumbItems.ToArray();
        Assert.DoesNotContain(items, x => x.Text == "Search");
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
    public void RemoveVirtualSegments_DoesNotFireEventWhenAlreadyEmpty()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);
        var fired = false;
        svc.ChangeEvent += (_, _) => fired = true;

        svc.RemoveVirtualSegments();

        Assert.False(fired);
    }
}
