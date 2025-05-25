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
}