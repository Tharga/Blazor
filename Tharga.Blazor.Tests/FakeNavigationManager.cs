using Microsoft.AspNetCore.Components;

namespace Tharga.Blazor.Tests;

internal class FakeNavigationManager : NavigationManager
{
    public FakeNavigationManager(string uri = "https://localhost/")
    {
        Initialize("https://localhost/", uri);
    }

    public void ChangeUri(string uri)
    {
        Uri = uri;
        NotifyLocationChanged(false);
    }

    protected override void NavigateToCore(string uri, NavigationOptions options) { }
}
