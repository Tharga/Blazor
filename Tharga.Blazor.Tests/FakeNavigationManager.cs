using Microsoft.AspNetCore.Components;

namespace Tharga.Blazor.Tests;

internal class FakeNavigationManager : NavigationManager
{
    public FakeNavigationManager(string uri = "https://localhost/", string baseUri = "https://localhost/")
    {
        Initialize(baseUri, uri);
    }

    public void ChangeUri(string uri)
    {
        Uri = uri;
        NotifyLocationChanged(false);
    }

    protected override void NavigateToCore(string uri, NavigationOptions options) { }
}
