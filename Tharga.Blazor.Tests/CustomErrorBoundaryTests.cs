using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Radzen;
using Tharga.Blazor.Framework;

namespace Tharga.Blazor.Tests;

public class CustomErrorBoundaryTests : BunitContext
{
    private const string CrashHeading = "Something went wrong!";
    private const string AccessDeniedHeading = "Access denied";
    private const string CorrelationText = "CorrelationId";
    private const string DeniedMessage = "You do not have access to this team.";
    private const string FailureMessage = "The database is on fire.";

    private readonly FakeLogger<CustomErrorBoundary> _logger = new();
    private readonly BlazorOptions _options = new();

    public CustomErrorBoundaryTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddRadzenComponents();
        Services.AddSingleton<ILogger<CustomErrorBoundary>>(_logger);
        Services.AddSingleton<IErrorBoundaryLogger, FakeErrorBoundaryLogger>();
        Services.AddSingleton<IOptions<BlazorOptions>>(new OptionsWrapper<BlazorOptions>(_options));
    }

    private static Exception Thrown(Exception exception)
    {
        try
        {
            throw exception;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    private IRenderedComponent<CustomErrorBoundary> RenderBoundary(Exception exception)
    {
        return Render<CustomErrorBoundary>(parameters => parameters
            .AddChildContent<ThrowingComponent>(child => child.Add(x => x.Exception, exception)));
    }

    [Fact]
    public void AccessDenied_RendersTheAccessDeniedPanel()
    {
        var boundary = RenderBoundary(Thrown(new UnauthorizedAccessException(DeniedMessage)));

        Assert.Contains(AccessDeniedHeading, boundary.Markup);
        Assert.Contains(DeniedMessage, boundary.Markup);
    }

    [Fact]
    public void AccessDenied_DoesNotRenderTheCrashHeading()
    {
        var boundary = RenderBoundary(Thrown(new UnauthorizedAccessException(DeniedMessage)));

        Assert.DoesNotContain(CrashHeading, boundary.Markup);
    }

    [Fact]
    public void AccessDenied_DoesNotRenderAStackTrace()
    {
        var exception = Thrown(new UnauthorizedAccessException(DeniedMessage));

        var boundary = RenderBoundary(exception);

        Assert.NotNull(exception.StackTrace);
        Assert.DoesNotContain(nameof(ThrowingComponent), boundary.Markup);
    }

    [Fact]
    public void AccessDenied_DoesNotRenderAStackTraceEvenWhenDetailsAreAllowed()
    {
        _options.ShowExceptionDetails = true;
        var exception = Thrown(new UnauthorizedAccessException(DeniedMessage));

        var boundary = RenderBoundary(exception);

        Assert.DoesNotContain(nameof(ThrowingComponent), boundary.Markup);
        Assert.DoesNotContain(CrashHeading, boundary.Markup);
    }

    [Fact]
    public void AccessDenied_DoesNotOfferACorrelationId()
    {
        var boundary = RenderBoundary(Thrown(new UnauthorizedAccessException(DeniedMessage)));

        Assert.DoesNotContain(CorrelationText, boundary.Markup);
    }

    [Fact]
    public void AccessDenied_IsLoggedAsWarning()
    {
        RenderBoundary(Thrown(new UnauthorizedAccessException(DeniedMessage)));

        var entry = Assert.Single(_logger.Entries);
        Assert.Equal(LogLevel.Warning, entry.Level);
        Assert.IsType<UnauthorizedAccessException>(entry.Exception);
    }

    [Fact]
    public void OtherException_RendersTheCrashPanel()
    {
        var boundary = RenderBoundary(Thrown(new InvalidOperationException(FailureMessage)));

        Assert.Contains(CrashHeading, boundary.Markup);
        Assert.Contains(CorrelationText, boundary.Markup);
        Assert.DoesNotContain(AccessDeniedHeading, boundary.Markup);
    }

    [Fact]
    public void OtherException_IsLoggedAsError()
    {
        RenderBoundary(Thrown(new InvalidOperationException(FailureMessage)));

        var entry = Assert.Single(_logger.Entries);
        Assert.Equal(LogLevel.Error, entry.Level);
        Assert.IsType<InvalidOperationException>(entry.Exception);
    }

    [Fact]
    public void WithoutShowExceptionDetails_NeitherMessageNorStackTraceIsRendered()
    {
        var exception = Thrown(new InvalidOperationException(FailureMessage));

        var boundary = RenderBoundary(exception);

        Assert.NotNull(exception.StackTrace);
        Assert.DoesNotContain(FailureMessage, boundary.Markup);
        Assert.DoesNotContain(nameof(ThrowingComponent), boundary.Markup);
    }

    [Fact]
    public void WithShowExceptionDetails_MessageAndStackTraceAreRendered()
    {
        _options.ShowExceptionDetails = true;

        var boundary = RenderBoundary(Thrown(new InvalidOperationException(FailureMessage)));

        Assert.Contains(FailureMessage, boundary.Markup);
        Assert.Contains(nameof(ThrowingComponent), boundary.Markup);
    }

    [Fact]
    public void ErrorContent_ReplacesTheWholePanel()
    {
        var boundary = Render<CustomErrorBoundary>(parameters => parameters
            .AddChildContent<ThrowingComponent>(child => child.Add(x => x.Exception, Thrown(new InvalidOperationException(FailureMessage))))
            .Add(x => x.ErrorContent, _ => builder => builder.AddMarkupContent(0, "<p>host-supplied</p>")));

        Assert.Contains("host-supplied", boundary.Markup);
        Assert.DoesNotContain(CrashHeading, boundary.Markup);
        Assert.DoesNotContain(CorrelationText, boundary.Markup);
    }

    [Fact]
    public void EveryException_CarriesACorrelationIdIntoTheLog()
    {
        var exception = Thrown(new InvalidOperationException(FailureMessage));

        RenderBoundary(exception);

        Assert.True(exception.Data.Contains("CorrelationId"));
        Assert.IsType<Guid>(exception.Data["CorrelationId"]);
    }

    [Fact]
    public void NoException_RendersTheChildContent()
    {
        var boundary = Render<CustomErrorBoundary>(parameters => parameters
            .AddChildContent("<p>all is well</p>"));

        Assert.Contains("all is well", boundary.Markup);
        Assert.Empty(_logger.Entries);
    }
}

internal class FakeErrorBoundaryLogger : IErrorBoundaryLogger
{
    public ValueTask LogErrorAsync(Exception exception) => ValueTask.CompletedTask;
}
