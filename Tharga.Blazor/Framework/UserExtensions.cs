using Microsoft.AspNetCore.Components.Authorization;

namespace Tharga.Blazor.Framework;

public static class UserExtensions
{
    public static string GetEmail(this AuthenticationState context)
    {
        return context.User.Claims.FirstOrDefault(x => x.Type == "emails")?.Value?.Replace("[\"", "").Replace("\"]", "");
    }
}