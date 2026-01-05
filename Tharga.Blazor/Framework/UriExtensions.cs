namespace Tharga.Blazor.Framework;

[Obsolete("Use from Tharga.Toolkit in version 1.15.17+")]
public static class UriExtensions
{
    [Obsolete("Use from Tharga.Toolkit in version 1.15.17+")]
    public static Uri RemoveQuery(this Uri uri)
    {
        var builder = new UriBuilder(uri)
        {
            Query = string.Empty
        };

        return builder.Uri;
    }

    [Obsolete("Use from Tharga.Toolkit in version 1.15.17+")]
    public static IEnumerable<string> GetQueryValue(this Uri uri, string name)
    {
        var query = uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in query)
        {
            var kvp = part.Split('=', 2);

            if (kvp.Length == 2 && kvp[0] == name)
            {
                yield return Uri.UnescapeDataString(kvp[1]);
            }
        }
    }
}