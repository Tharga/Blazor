using System.Reflection;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Radzen;
using Tharga.Blazor.Features.BreadCrumbs;
using Tharga.Blazor.Framework;

namespace Tharga.Blazor.Tests;

public class EventUnsubscribeTests : BunitContext
{
    public EventUnsubscribeTests()
    {
        Services.AddRadzenComponents();
        Services.AddSingleton<IOptions<BlazorOptions>>(new OptionsWrapper<BlazorOptions>(new BlazorOptions()));
        Services.AddScoped<BreadCrumbService>();
        SetRendererInfo(new RendererInfo("Server", isInteractive: true));
    }

    [Fact]
    public async Task BreadCrumbs_Dispose_UnsubscribesFromChangeEvent()
    {
        var service = Services.GetRequiredService<BreadCrumbService>();
        var before = HandlerCount(service, typeof(BreadCrumbService), "ChangeEvent");

        Render<BreadCrumbs>();
        Assert.Equal(before + 1, HandlerCount(service, typeof(BreadCrumbService), "ChangeEvent"));

        await DisposeComponentsAsync();

        Assert.Equal(before, HandlerCount(service, typeof(BreadCrumbService), "ChangeEvent"));
    }

    [Fact]
    public async Task Title_Dispose_UnsubscribesFromLocationChanged()
    {
        var navigationManager = Services.GetRequiredService<NavigationManager>();
        var before = HandlerCount(navigationManager, typeof(NavigationManager), "_locationChanged");

        Render<Title>();
        Assert.Equal(before + 1, HandlerCount(navigationManager, typeof(NavigationManager), "_locationChanged"));

        await DisposeComponentsAsync();

        Assert.Equal(before, HandlerCount(navigationManager, typeof(NavigationManager), "_locationChanged"));
    }

    /// <summary>
    /// Counts the handlers behind an event by reading its backing delegate field; there is no public way to ask.
    /// </summary>
    private static int HandlerCount(object target, Type declaringType, string fieldName)
    {
        var field = declaringType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? throw new InvalidOperationException($"No field '{fieldName}' on {declaringType.Name}.");
        return (field.GetValue(target) as Delegate)?.GetInvocationList().Length ?? 0;
    }
}
