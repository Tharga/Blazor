using Microsoft.AspNetCore.Components;
using Tharga.Toolkit;

namespace Tharga.Blazor.Features.BreadCrumbs;

public class BreadCrumbService
{
    private Crumb[] _segments = [];
    private Crumb[] _virtualSegments = [];
    private Crumb[] _autoVirtualSegments = [];
    private readonly Dictionary<string, Dictionary<string, Modifier>> _modifiers = new ();
    private readonly HashSet<string> _virtualSegmentQueryParams = [];
    private string _lastNormalizedUri;
    private readonly NavigationManager _navigationManager;
    private readonly IBreadCrumbTextProvider _textProvider;

    public BreadCrumbService(NavigationManager navigationManager)
        : this(navigationManager, null)
    {
    }

    public BreadCrumbService(NavigationManager navigationManager, IBreadCrumbTextProvider textProvider)
    {
        _navigationManager = navigationManager;
        _textProvider = textProvider;
        navigationManager.LocationChanged += (s, _) => { Build(navigationManager, s); };

        Build(navigationManager, this);
    }

    private static string NormalizeUri(string uri)
    {
        var idx = uri.IndexOf('?');
        return idx >= 0 ? uri.Substring(0, idx) : uri;
    }

    private void Build(NavigationManager navigationManager, object s)
    {
        var normalizedUri = NormalizeUri(navigationManager.Uri);
        if (normalizedUri != _lastNormalizedUri)
        {
            _virtualSegments = [];
            _lastNormalizedUri = normalizedUri;
        }

        var parts = navigationManager.Uri
            .Split('/')
            .Skip(3)
            .ToArray();

        _segments = parts
            .Where(x => !string.IsNullOrEmpty(x))
            .Select((text, i) =>
            {
                var path = string.Join("/", parts.Take(i + 1));

                var pos = text.IndexOf("?", StringComparison.Ordinal);
                if (pos > 0) text = text.Substring(0, pos);

                return new Crumb { Text = text, Path = path, Segment = text, SegmentPath = path };
            })
            .ToArray();

        if (_modifiers.TryGetValue(normalizedUri, out var modifiers))
        {
            _segments = _segments.Select(x =>
                {
                    if (!modifiers.TryGetValue(x.Text, out var item)) return x;

                    var crumb = x with { TextOverride = item.Text };
                    switch (item.Modifyer)
                    {
                        case Modifyer.Remove:
                            return null;
                        case Modifyer.Unlink:
                            return crumb with { Path = null };
                        case Modifyer.Relink:
                            return crumb with { Path = item.RelinkUrl };
                        case null:
                            return crumb;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                })
                .Where(x => x != null)
                .ToArray();
        }

        _autoVirtualSegments = BuildAutoVirtualSegments(navigationManager.Uri);

        ChangeEvent?.Invoke(s, EventArgs.Empty);
    }

    private Crumb[] BuildAutoVirtualSegments(string uri)
    {
        if (_virtualSegmentQueryParams.Count == 0) return [];

        var idx = uri.IndexOf('?');
        if (idx < 0) return [];

        var result = new List<Crumb>();
        foreach (var part in uri.Substring(idx + 1).Split('&'))
        {
            var eq = part.IndexOf('=');
            if (eq < 0) continue;
            var key = Uri.UnescapeDataString(part.Substring(0, eq));
            var value = Uri.UnescapeDataString(part.Substring(eq + 1));
            if (_virtualSegmentQueryParams.Contains(key))
                result.Add(new Crumb { Text = value, Path = null });
        }
        return [.. result];
    }

    public void RegisterVirtualSegmentQueryParam(string paramName)
    {
        if (_virtualSegmentQueryParams.Add(paramName))
            Build(_navigationManager, this);
    }

    public event EventHandler<EventArgs> ChangeEvent;

    public IEnumerable<BreadCrumb> BreadCrumbItems
    {
        get
        {
            var all = _segments.Concat(_autoVirtualSegments).Concat(_virtualSegments).ToArray();
            return all.Select((crumb, index) =>
            {
                var disabled = index == all.Length - 1;

                return new BreadCrumb
                {
                    Text = GetText(crumb),
                    Path = disabled ? null : crumb.Path
                };
            });
        }
    }

    private string GetText(Crumb crumb)
    {
        if (crumb.TextOverride != null) return crumb.TextOverride;

        if (crumb.Segment != null)
        {
            var text = _textProvider?.GetText(crumb.Segment, crumb.SegmentPath);
            if (text != null) return text;
        }

        if (string.IsNullOrEmpty(crumb.Text)) return crumb.Text;
        return crumb.Text.Substring(0, 1).ToUpper() + crumb.Text.Substring(1);
    }

    /// <summary>
    /// Raises ChangeEvent so the breadcrumbs re-render, for example after the language changed.
    /// </summary>
    public void Refresh()
    {
        ChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public void AddVirtualSegment(string text, string path = null)
    {
        _virtualSegments = [.. _virtualSegments, new Crumb { Text = text, Path = path }];
        ChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveVirtualSegments()
    {
        if (_virtualSegments.Length == 0) return;
        _virtualSegments = [];
        ChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Shows a different text for a path segment on the current URL. The segment keeps its position and link,
    /// and the text is shown as given. Pass null to go back to the default text.
    /// </summary>
    public void SetSegmentText(string segment, string text)
    {
        Modify(segment, x => x with { Text = text });
    }

    public void RelinkSegment(string text, string url)
    {
        Modify(text, x => x.Modifyer == null ? x with { Modifyer = Modifyer.Relink, RelinkUrl = url } : x);
    }

    public void UnlinkSegment(string text)
    {
        Modify(text, x => x.Modifyer == null ? x with { Modifyer = Modifyer.Unlink } : x);
    }

    public void RemoveSegment(string text)
    {
        Modify(text, x => x.Modifyer == null ? x with { Modifyer = Modifyer.Remove } : x);
    }

    private void Modify(string segment, Func<Modifier, Modifier> change)
    {
        var key = NormalizeUri(_navigationManager.Uri);
        if (!_modifiers.TryGetValue(key, out var modifiers))
        {
            modifiers = new Dictionary<string, Modifier>(StringComparer.InvariantCultureIgnoreCase);
            _modifiers.Add(key, modifiers);
        }

        var current = modifiers.GetValueOrDefault(segment) ?? new Modifier();
        var updated = change(current);
        if (updated == current) return;

        modifiers[segment] = updated;
        Build(_navigationManager, this);
    }

    record Crumb
    {
        public required string Text { get; init; }
        public string Path { get; init; }
        public string TextOverride { get; init; }

        /// <summary>The URL segment and its route path; null for virtual segments.</summary>
        public string Segment { get; init; }
        public string SegmentPath { get; init; }
    }

    record Modifier
    {
        public Modifyer? Modifyer { get; init; }
        public string RelinkUrl { get; init; }
        public string Text { get; init; }
    }

    public enum Modifyer
    {
        Remove,
        Unlink,
        Relink
    }
}
