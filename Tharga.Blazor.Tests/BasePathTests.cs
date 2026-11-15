using Tharga.Blazor.Features.BreadCrumbs;
using Tharga.Blazor.Framework;

namespace Tharga.Blazor.Tests;

public class BasePathTests
{
    private const string SubPathBase = "https://host/myapp/";

    [Fact]
    public void BreadCrumbs_UnderSubPath_ExcludeTheBasePath()
    {
        var nav = new FakeNavigationManager($"{SubPathBase}customers/42", SubPathBase);
        var svc = new BreadCrumbService(nav);

        var items = svc.BreadCrumbItems.ToArray();

        Assert.Equal(["Customers", "42"], items.Select(x => x.Text));
        Assert.Equal(["customers", null], items.Select(x => x.Path));
    }

    [Fact]
    public void BreadCrumbs_AtRootOfSubPath_HaveNoItems()
    {
        var nav = new FakeNavigationManager(SubPathBase, SubPathBase);
        var svc = new BreadCrumbService(nav);

        Assert.Empty(svc.BreadCrumbItems);
    }

    [Fact]
    public void BreadCrumbs_AtRoot_KeepWorkingWithoutBasePath()
    {
        var nav = new FakeNavigationManager("https://localhost/developer/log");
        var svc = new BreadCrumbService(nav);

        Assert.Equal(["developer", null], svc.BreadCrumbItems.Select(x => x.Path));
    }

    [Fact]
    public void BreadCrumbs_IgnoreQueryAndFragment()
    {
        var nav = new FakeNavigationManager($"{SubPathBase}customers/42?id=1#top", SubPathBase);
        var svc = new BreadCrumbService(nav);

        Assert.Equal(["Customers", "42"], svc.BreadCrumbItems.Select(x => x.Text));
    }

    [Fact]
    public void Segments_UnderSubPath_ExcludeTheBasePath()
    {
        var nav = new FakeNavigationManager($"{SubPathBase}customers/42?x=1", SubPathBase);

        Assert.Equal(["customers", "42"], NavigationPath.Segments(nav));
    }

    [Fact]
    public void Segments_AtRoot_AreEmpty()
    {
        var nav = new FakeNavigationManager(SubPathBase, SubPathBase);

        Assert.Empty(NavigationPath.Segments(nav));
    }
}
