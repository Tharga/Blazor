using System.Security.Claims;

namespace Tharga.Blazor.Features.Authentication;

public static class ClaimsIdentityExtensions
{
    public static string ToEmail(this ClaimsPrincipal item)
    {
        return item.Claims.ToEmail();
    }

    public static string ToEmail(this ClaimsIdentity item)
    {
        return item.Claims.ToEmail();
    }

    public static string ToEmail(this IEnumerable<Claim> claims)
    {
        var enumerable = claims as Claim[] ?? claims.ToArray();

        var email = enumerable.FirstOrDefault(x => x.Type == "email")?.Value ??
                    enumerable.FirstOrDefault(x => x.Type == "emails")?.Value.Replace("[\"", "").Replace("\"]", "");

        if (string.IsNullOrEmpty(email))
        {
            var preferredUsername = enumerable.FirstOrDefault(x => x.Type == "preferred_username")?.Value;
            if (preferredUsername?.Contains("@") ?? false) email = preferredUsername;
        }

        return email;
    }

    public static string ToEmailDomain(this ClaimsIdentity item)
    {
        var email = item?.ToEmail();
        if (string.IsNullOrEmpty(email)) return null;

        try
        {
            return new System.Net.Mail.MailAddress(email).Host;
        }
        catch (FormatException)
        {
            return null;
        }
    }

    public static void AddRoleForDomain(this ClaimsPrincipal claimsPrincipal, string role, params string[] domains)
    {
        if (claimsPrincipal == null || claimsPrincipal.Identity == null) throw new ArgumentNullException(nameof(claimsPrincipal), "ClaimsPrincipal or its Identity cannot be null.");

        var claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;

        var emailDomain = claimsIdentity.ToEmailDomain();
        if (string.IsNullOrEmpty(emailDomain)) return;

        var roleClaims = claimsIdentity.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Select(x => x.Value);

        if (domains.Any(d => emailDomain.Equals(d, StringComparison.InvariantCultureIgnoreCase))
            && !roleClaims.Contains(role, StringComparer.InvariantCultureIgnoreCase))
        {
            claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role));
        }
    }
}