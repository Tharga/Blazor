using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tharga.Team.Blazor.Features.Authentication;

namespace Tharga.Team.Blazor.Tests;

public class ThargaAuthRegistrationTests
{
    private const string ValidAzureAdConfig = """
        {
            "AzureAd": {
                "Authority": "https://test.ciamlogin.com/test",
                "ClientId": "test-client-id",
                "TenantId": "test-tenant-id",
                "CallbackPath": "/signin-oidc"
            }
        }
        """;

    private static WebApplicationBuilder CreateBuilderWithConfig(string json)
    {
        var builder = WebApplication.CreateBuilder();
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json));
        builder.Configuration.AddJsonStream(stream);
        return builder;
    }

    private static WebApplicationBuilder CreateBuilderWithoutConfig()
    {
        return WebApplication.CreateBuilder();
    }

    [Fact]
    public void AddThargaAuth_RegistersAuthenticationServices()
    {
        var builder = CreateBuilderWithConfig(ValidAzureAdConfig);

        builder.AddThargaAuth();

        var provider = builder.Services.BuildServiceProvider();
        var authService = provider.GetService<IAuthenticationService>();
        Assert.NotNull(authService);
    }

    [Fact]
    public async Task AddThargaAuth_RegistersDefaultCookieScheme()
    {
        var builder = CreateBuilderWithConfig(ValidAzureAdConfig);

        builder.AddThargaAuth();

        var provider = builder.Services.BuildServiceProvider();
        var schemeProvider = provider.GetRequiredService<IAuthenticationSchemeProvider>();
        var defaultScheme = await schemeProvider.GetDefaultAuthenticateSchemeAsync();
        Assert.NotNull(defaultScheme);
        Assert.Equal(CookieAuthenticationDefaults.AuthenticationScheme, defaultScheme.Name);
    }

    [Fact]
    public async Task AddThargaAuth_RegistersOidcChallengeScheme()
    {
        var builder = CreateBuilderWithConfig(ValidAzureAdConfig);

        builder.AddThargaAuth();

        var provider = builder.Services.BuildServiceProvider();
        var schemeProvider = provider.GetRequiredService<IAuthenticationSchemeProvider>();
        var challengeScheme = await schemeProvider.GetDefaultChallengeSchemeAsync();
        Assert.NotNull(challengeScheme);
        Assert.Equal(OpenIdConnectDefaults.AuthenticationScheme, challengeScheme.Name);
    }

    [Fact]
    public void AddThargaAuth_RegistersHttpContextAccessor()
    {
        var builder = CreateBuilderWithConfig(ValidAzureAdConfig);

        builder.AddThargaAuth();

        var provider = builder.Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IHttpContextAccessor>());
    }

    [Fact]
    public void AddThargaAuth_RegistersAuthorizationServices()
    {
        var builder = CreateBuilderWithConfig(ValidAzureAdConfig);

        builder.AddThargaAuth();

        var provider = builder.Services.BuildServiceProvider();
        var authzService = provider.GetService<Microsoft.AspNetCore.Authorization.IAuthorizationService>();
        Assert.NotNull(authzService);
    }

    [Fact]
    public void AddThargaAuth_RegistersOptionsAsSingleton()
    {
        var builder = CreateBuilderWithConfig(ValidAzureAdConfig);

        builder.AddThargaAuth();

        var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetService<ThargaAuthOptions>();
        Assert.NotNull(options);
    }

    [Fact]
    public void AddThargaAuth_DefaultOptions_HaveExpectedValues()
    {
        var builder = CreateBuilderWithConfig(ValidAzureAdConfig);

        builder.AddThargaAuth();

        var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<ThargaAuthOptions>();
        Assert.Equal("/login", options.LoginPath);
        Assert.Equal("/logout", options.LogoutPath);
        Assert.True(options.ValidateConfiguration);
    }

    [Fact]
    public void AddThargaAuth_CustomOptions_AreApplied()
    {
        var builder = CreateBuilderWithConfig(ValidAzureAdConfig);

        builder.AddThargaAuth(o =>
        {
            o.LoginPath = "/sign-in";
            o.LogoutPath = "/sign-out";
        });

        var provider = builder.Services.BuildServiceProvider();
        var options = provider.GetRequiredService<ThargaAuthOptions>();
        Assert.Equal("/sign-in", options.LoginPath);
        Assert.Equal("/sign-out", options.LogoutPath);
    }

    [Fact]
    public void AddThargaAuth_ThrowsWhenAzureAdSectionMissing()
    {
        var builder = CreateBuilderWithoutConfig();

        var ex = Assert.Throws<InvalidOperationException>(() => builder.AddThargaAuth());
        Assert.Contains("AzureAd", ex.Message);
    }

    [Fact]
    public void AddThargaAuth_SkipsValidationWhenDisabled()
    {
        var builder = CreateBuilderWithoutConfig();

        builder.AddThargaAuth(o => o.ValidateConfiguration = false);

        var provider = builder.Services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<ThargaAuthOptions>());
    }

    [Fact]
    public void UseThargaAuth_MapsLoginEndpoint()
    {
        var builder = CreateBuilderWithConfig(ValidAzureAdConfig);
        builder.AddThargaAuth();
        var app = builder.Build();

        app.UseThargaAuth();

        var endpoints = GetRouteEndpoints(app);
        Assert.Contains(endpoints, e => e.RoutePattern.RawText == "/login");
    }

    [Fact]
    public void UseThargaAuth_MapsLogoutEndpoint()
    {
        var builder = CreateBuilderWithConfig(ValidAzureAdConfig);
        builder.AddThargaAuth();
        var app = builder.Build();

        app.UseThargaAuth();

        var endpoints = GetRouteEndpoints(app);
        Assert.Contains(endpoints, e => e.RoutePattern.RawText == "/logout");
    }

    [Fact]
    public void UseThargaAuth_MapsCustomEndpointPaths()
    {
        var builder = CreateBuilderWithConfig(ValidAzureAdConfig);
        builder.AddThargaAuth(o =>
        {
            o.LoginPath = "/sign-in";
            o.LogoutPath = "/sign-out";
        });
        var app = builder.Build();

        app.UseThargaAuth();

        var endpoints = GetRouteEndpoints(app);
        Assert.Contains(endpoints, e => e.RoutePattern.RawText == "/sign-in");
        Assert.Contains(endpoints, e => e.RoutePattern.RawText == "/sign-out");
    }

    private static List<Microsoft.AspNetCore.Routing.RouteEndpoint> GetRouteEndpoints(WebApplication app)
    {
        return ((Microsoft.AspNetCore.Routing.IEndpointRouteBuilder)app)
            .DataSources
            .SelectMany(ds => ds.Endpoints)
            .OfType<Microsoft.AspNetCore.Routing.RouteEndpoint>()
            .ToList();
    }
}
