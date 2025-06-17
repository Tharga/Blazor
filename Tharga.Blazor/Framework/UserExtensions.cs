using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Tharga.Blazor.Framework;

public static class UserExtensions
{
    public static string GetEmail(this AuthenticationState context)
    {
        return context.User.GetEmail();
    }

    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "emails")?.Value?.Replace("[\"", "").Replace("\"]", "");
    }

    public static string GetKey(this AuthenticationState context)
    {
        return context.User.GetKey();
    }

    public static string GetKey(this ClaimsPrincipal claimsPrincipal)
    {
        var key = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(key))
        {
            key = claimsPrincipal.FindFirst("sub")?.Value;
        }

        return key;
    }

}