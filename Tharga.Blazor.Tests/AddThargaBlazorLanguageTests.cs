using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tharga.Blazor.Framework;
using Tharga.Blazor.Framework.Localization;
using Tharga.Toolkit;

namespace Tharga.Blazor.Tests;

public class AddThargaBlazorLanguageTests
{
    private static IConfiguration Configuration(string value)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Tharga:Blazor:Language"] = value
            })
            .Build();
    }

    private static BlazorOptions Resolve(IServiceCollection services)
    {
        return services.BuildServiceProvider().GetRequiredService<IOptions<BlazorOptions>>().Value;
    }

    [Fact]
    public void WithoutConfiguration_LanguageIsUnset()
    {
        var services = new ServiceCollection();

        services.AddThargaBlazor();

        Assert.Null(Resolve(services).Language);
    }

    [Fact]
    public void ConfigurationBindsLanguage()
    {
        var services = new ServiceCollection();

        services.AddThargaBlazor(configuration: Configuration("Sv"));

        Assert.Equal(Language.Sv, Resolve(services).Language);
    }

    [Fact]
    public void CodeOverridesConfiguration()
    {
        var services = new ServiceCollection();

        services.AddThargaBlazor(o => o.Language = Language.En, Configuration("Sv"));

        Assert.Equal(Language.En, Resolve(services).Language);
    }

    [Fact]
    public void ACallbackThatSetsSomethingElseDoesNotClobberTheConfiguredValue()
    {
        var services = new ServiceCollection();

        services.AddThargaBlazor(o => o.Title = "My App", Configuration("Sv"));

        var options = Resolve(services);
        Assert.Equal(Language.Sv, options.Language);
        Assert.Equal("My App", options.Title);
    }

    [Fact]
    public void TheDefaultLanguageProviderHasNoOpinion()
    {
        var services = new ServiceCollection();
        services.AddThargaBlazor();

        var provider = services.BuildServiceProvider().CreateScope().ServiceProvider
            .GetRequiredService<IBlazorLanguageProvider>();

        Assert.Null(provider.GetLanguage());
    }

    [Fact]
    public void AHostLanguageProviderReplacesTheDefault()
    {
        var services = new ServiceCollection();
        services.AddScoped<IBlazorLanguageProvider>(_ => new FakeLanguageProvider { Language = Language.Sv });

        services.AddThargaBlazor();

        var provider = services.BuildServiceProvider().CreateScope().ServiceProvider
            .GetRequiredService<IBlazorLanguageProvider>();

        Assert.Equal(Language.Sv, provider.GetLanguage());
    }

    [Fact]
    public void TheResolverIsRegisteredAndUsesTheHostProvider()
    {
        var services = new ServiceCollection();
        services.AddScoped<IBlazorLanguageProvider>(_ => new FakeLanguageProvider { Language = Language.Sv });
        services.AddThargaBlazor();

        var resolver = services.BuildServiceProvider().CreateScope().ServiceProvider
            .GetRequiredService<LanguageResolver>();

        Assert.Equal(Language.Sv, resolver.Resolve());
    }
}
