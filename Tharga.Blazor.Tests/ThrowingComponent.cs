using Microsoft.AspNetCore.Components;

namespace Tharga.Blazor.Tests;

internal class ThrowingComponent : ComponentBase
{
    [Parameter]
    public Exception Exception { get; set; } = new InvalidOperationException("Boom");

    protected override void OnInitialized()
    {
        throw Exception;
    }
}
