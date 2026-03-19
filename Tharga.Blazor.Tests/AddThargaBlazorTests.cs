using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tharga.Blazor.Features.BreadCrumbs;
using Tharga.Blazor.Framework;

namespace Tharga.Blazor.Tests;

public class AddThargaBlazorTests
{
    [Fact]
    public void AddThargaBlazor_RegistersBreadCrumbService()
    {
        var services = new ServiceCollection();
        services.AddScoped<Microsoft.AspNetCore.Components.NavigationManager>(_ => new FakeNavigationManager());

        services.AddThargaBlazor();

        var provider = services.BuildServiceProvider();
        var service = provider.GetService<BreadCrumbService>();
        Assert.NotNull(service);
    }

    [Fact]
    public void AddThargaBlazor_RegistersBlazoredLocalStorage()
    {
        var services = new ServiceCollection();

        services.AddThargaBlazor();

        Assert.Contains(services, d => d.ServiceType == typeof(ILocalStorageService));
    }

    [Fact]
    public void AddThargaBlazor_RegistersBlazorOptionsWithTitle()
    {
        var services = new ServiceCollection();

        services.AddThargaBlazor(o => o.Title = "My App");

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<BlazorOptions>>();
        Assert.Equal("My App", options.Value.Title);
    }

    [Fact]
    public void AddThargaBlazor_WithoutTitle_RegistersBlazorOptionsWithNullTitle()
    {
        var services = new ServiceCollection();

        services.AddThargaBlazor();

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<BlazorOptions>>();
        Assert.Null(options.Value.Title);
    }

    [Fact]
    public void AddThargaBlazor_WithConfiguration_BindsTitleFromConfig()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Tharga:Blazor:Title"] = "Config Title"
            })
            .Build();

        services.AddThargaBlazor(configuration: configuration);

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<BlazorOptions>>();
        Assert.Equal("Config Title", options.Value.Title);
    }

    [Fact]
    public void AddThargaBlazor_CodeOverridesConfiguration()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Tharga:Blazor:Title"] = "Config Title"
            })
            .Build();

        services.AddThargaBlazor(o => o.Title = "Code Title", configuration);

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<BlazorOptions>>();
        Assert.Equal("Code Title", options.Value.Title);
    }
}
