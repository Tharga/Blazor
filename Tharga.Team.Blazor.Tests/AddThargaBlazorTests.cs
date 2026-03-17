using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tharga.Blazor.Framework;
using Tharga.Team.Blazor.Framework;

namespace Tharga.Team.Blazor.Tests;

public class AddThargaBlazorTests
{
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
}
