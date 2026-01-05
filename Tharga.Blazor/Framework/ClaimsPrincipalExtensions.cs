using System.Globalization;
using System.Security.Claims;
using Tharga.Toolkit;

namespace Tharga.Blazor.Framework;

public static class ClaimsPrincipalExtensions
{
    public static string ToShortNameFromEmail(this ClaimsPrincipal claimsPrincipal)
    {
        var email = claimsPrincipal.GetEmail();
        var atIndex = email.IndexOf('@');
        var username = atIndex >= 0 ? email.Substring(0, atIndex) : email;
        return username;
    }

    public static string ToFullNameFromEmail(this ClaimsPrincipal claimsPrincipal)
    {
        var email = claimsPrincipal.GetEmail();
        var atIndex = email.IndexOf('@');
        var name = atIndex >= 0 ? email.Substring(0, atIndex) : email;
        name = name.Replace(".", " ");
        name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
        return name;
    }
}