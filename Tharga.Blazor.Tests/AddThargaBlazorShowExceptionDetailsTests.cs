using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tharga.Blazor.Framework;

namespace Tharga.Blazor.Tests;

public class AddThargaBlazorShowExceptionDetailsTests
{
    private static IConfiguration Configuration(string value)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Tharga:Blazor:ShowExceptionDetails"] = value
            })
            .Build();
    }

    private static BlazorOptions Resolve(IServiceCollection services)
    {
        return services.BuildServiceProvider().GetRequiredService<IOptions<BlazorOptions>>().Value;
    }

    [Fact]
    public void WithoutConfiguration_ShowExceptionDetailsIsUnset()
    {
        var services = new ServiceCollection();

        services.AddThargaBlazor();

        Assert.Null(Resolve(services).ShowExceptionDetails);
    }

    [Fact]
    public void ConfigurationBindsShowExceptionDetails()
    {
        var services = new ServiceCollection();

        services.AddThargaBlazor(configuration: Configuration("true"));

        Assert.True(Resolve(services).ShowExceptionDetails);
    }

    [Fact]
    public void CodeOverridesConfiguration()
    {
        var services = new ServiceCollection();

        services.AddThargaBlazor(o => o.ShowExceptionDetails = false, Configuration("true"));

        Assert.False(Resolve(services).ShowExceptionDetails);
    }

    [Fact]
    public void ACallbackThatSetsSomethingElseDoesNotClobberTheConfiguredValue()
    {
        var services = new ServiceCollection();

        services.AddThargaBlazor(o => o.Title = "My App", Configuration("true"));

        var options = Resolve(services);
        Assert.True(options.ShowExceptionDetails);
        Assert.Equal("My App", options.Title);
    }
}
