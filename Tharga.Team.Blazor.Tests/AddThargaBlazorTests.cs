using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tharga.Blazor.Framework;
using Tharga.Team.Blazor.Framework;

namespace Tharga.Team.Blazor.Tests;

public class AddThargaTeamBlazorTests
{
    [Fact]
    public void AddThargaTeamBlazor_RegistersBlazorOptionsWithTitle()
    {
        var services = new ServiceCollection();

        services.AddThargaTeamBlazor(o => o.Title = "My App");

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<BlazorOptions>>();
        Assert.Equal("My App", options.Value.Title);
    }

    [Fact]
    public void AddThargaTeamBlazor_WithoutTitle_RegistersBlazorOptionsWithNullTitle()
    {
        var services = new ServiceCollection();

        services.AddThargaTeamBlazor();

        var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<BlazorOptions>>();
        Assert.Null(options.Value.Title);
    }

    [Fact]
    public void AddThargaTeamBlazor_RegistersThargaBlazorOptions()
    {
        var services = new ServiceCollection();

        services.AddThargaTeamBlazor(o =>
        {
            o.Title = "Team App";
            o.AutoCreateFirstTeam = true;
        });

        var provider = services.BuildServiceProvider();
        var teamOptions = provider.GetRequiredService<IOptions<ThargaBlazorOptions>>();
        Assert.Equal("Team App", teamOptions.Value.Title);
        Assert.True(teamOptions.Value.AutoCreateFirstTeam);
    }
}
