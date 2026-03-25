using Microsoft.AspNetCore.Components.Web;

namespace Tharga.Blazor.Framework;

public static class CustomRenderModes
{
    public static readonly InteractiveServerRenderMode StandardMode = new(false);
    public static readonly InteractiveAutoRenderMode InteractiveAutoRenderModeNoPreRender = new(false);
    public static readonly InteractiveServerRenderMode InteractiveServerRenderModeNoPreRender = new(false);
    public static readonly InteractiveWebAssemblyRenderMode InteractiveWebAssemblyRenderModeNoPreRender = new(false);
}