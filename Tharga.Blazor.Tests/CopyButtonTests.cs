using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Radzen;
using Tharga.Blazor.Framework.Buttons;

namespace Tharga.Blazor.Tests;

public class CopyButtonTests : BunitContext
{
    private const string CopyFunction = "clipboard.copyText";
    private readonly NotificationService _notifications = new();

    public CopyButtonTests()
    {
        Services.AddSingleton(_notifications);
        SetRendererInfo(new RendererInfo("Server", isInteractive: true));
    }

    private IRenderedComponent<CopyButton> RenderButton(string content = "text")
    {
        return Render<CopyButton>(parameters => parameters.Add(x => x.Content, content));
    }

    [Fact]
    public async Task Click_WhenScriptIsMissing_DoesNotThrowAndWarns()
    {
        JSInterop.SetupVoid(CopyFunction, _ => true).SetException(new JSException("Could not find 'clipboard.copyText'"));
        var button = RenderButton();

        await button.Find("button").ClickAsync();

        Assert.Equal(NotificationSeverity.Warning, _notifications.Messages.Single().Severity);
    }

    [Fact]
    public async Task Click_WhenCircuitIsDisconnected_DoesNotThrow()
    {
        JSInterop.SetupVoid(CopyFunction, _ => true).SetException(new JSDisconnectedException("gone"));
        var button = RenderButton();

        await button.Find("button").ClickAsync();

        Assert.Equal(NotificationSeverity.Warning, _notifications.Messages.Single().Severity);
    }

    [Fact]
    public async Task Click_WhenCopySucceeds_NotifiesInfo()
    {
        JSInterop.SetupVoid(CopyFunction, _ => true).SetVoidResult();
        var button = RenderButton();

        await button.Find("button").ClickAsync();

        Assert.Equal(NotificationSeverity.Info, _notifications.Messages.Single().Severity);
    }
}
