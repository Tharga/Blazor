using Microsoft.AspNetCore.Components;

namespace Tharga.Blazor.Framework;

internal static class NavigationPath
{
    public static string[] Segments(NavigationManager navigationManager)
    {
        string relative;
        try
        {
            relative = navigationManager.ToBaseRelativePath(navigationManager.Uri);
        }
        catch (ArgumentException)
        {
            return [];
        }

        var end = relative.IndexOfAny(['?', '#']);
        if (end >= 0) relative = relative.Substring(0, end);

        return relative.Split('/', StringSplitOptions.RemoveEmptyEntries);
    }
}
