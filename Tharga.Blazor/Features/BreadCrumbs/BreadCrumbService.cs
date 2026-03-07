using Microsoft.AspNetCore.Components;
using Tharga.Toolkit;

namespace Tharga.Blazor.Features.BreadCrumbs;

public class BreadCrumbService
{
    private BreadCrumb[] _segments = [];
    private BreadCrumb[] _virtualSegments = [];
    private readonly Dictionary<string, List<Modifier>> _modifiers = new ();
    private string _lastNormalizedUri;
    private readonly NavigationManager _navigationManager;

    public BreadCrumbService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
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

                return new BreadCrumb { Text = text, Path = path };
            })
            .ToArray();

        if (_modifiers.TryGetValue(NormalizeUri(navigationManager.Uri), out var modifiers))
        {
            _segments = _segments.Select(x =>
                {
                    var item = modifiers.SingleOrDefault(y => x.Text.Equals(y.Text, StringComparison.InvariantCultureIgnoreCase));
                    switch (item?.Modifyer)
                    {
                        case Modifyer.Remove:
                            return null;
                        case Modifyer.Unlink:
                            return x with { Path = null };
                        case Modifyer.Relink:
                            return x with { Path = item.RelinkUrl };
                        case null:
                            return x;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                })
                .Where(x => x != null)
                .ToArray();
        }

        ChangeEvent?.Invoke(s, EventArgs.Empty);
    }

    public event EventHandler<EventArgs> ChangeEvent;

    public IEnumerable<BreadCrumb> BreadCrumbItems
    {
        get
        {
            var all = _segments.Concat(_virtualSegments).ToArray();
            return all.Select((path, index) =>
            {
                var text = path.Text.Substring(0, 1).ToUpper() + path.Text.Substring(1);
                var disabled = index == all.Length - 1;

                return new BreadCrumb
                {
                    Text = text,
                    Path = disabled ? null : path.Path
                };
            });
        }
    }

    public void AddVirtualSegment(string text)
    {
        _virtualSegments = [.. _virtualSegments, new BreadCrumb { Text = text, Path = null }];
        ChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveVirtualSegments()
    {
        if (_virtualSegments.Length == 0) return;
        _virtualSegments = [];
        ChangeEvent?.Invoke(this, EventArgs.Empty);
    }

    public void RelinkSegment(string text, string url)
    {
        var key = NormalizeUri(_navigationManager.Uri);
        if (_modifiers.TryGetValue(key, out var modifiers))
        {
            var item = modifiers.FirstOrDefault(x => x.Text == text);
            if (item == null)
            {
                modifiers.Add(new Modifier { Text = text, Modifyer = Modifyer.Relink, RelinkUrl = url });
                Build(_navigationManager, this);
            }
        }
        else
        {
            _modifiers.Add(key, [new Modifier { Text = text, Modifyer = Modifyer.Relink, RelinkUrl = url }]);
            Build(_navigationManager, this);
        }
    }

    public void UnlinkSegment(string text)
    {
        var key = NormalizeUri(_navigationManager.Uri);
        if (_modifiers.TryGetValue(key, out var modifiers))
        {
            var item = modifiers.FirstOrDefault(x => x.Text == text);
            if (item == null)
            {
                modifiers.Add(new Modifier { Text = text, Modifyer = Modifyer.Unlink });
                Build(_navigationManager, this);
            }
        }
        else
        {
            _modifiers.Add(key, [new Modifier { Text = text, Modifyer = Modifyer.Unlink }]);
            Build(_navigationManager, this);
        }
    }

    public void RemoveSegment(string text)
    {
        var key = NormalizeUri(_navigationManager.Uri);
        if (_modifiers.TryGetValue(key, out var modifiers))
        {
            var item = modifiers.FirstOrDefault(x => x.Text == text);
            if (item == null)
            {
                modifiers.Add(new Modifier { Text = text, Modifyer = Modifyer.Remove });
                Build(_navigationManager, this);
            }
        }
        else
        {
            _modifiers.Add(key, [new Modifier { Text = text, Modifyer = Modifyer.Remove }]);
            Build(_navigationManager, this);
        }
    }

    record Modifier
    {
        public required string Text { get; init; }
        public required Modifyer Modifyer { get; init; }
        public string RelinkUrl { get; init; }
    }

    public enum Modifyer
    {
        Remove,
        Unlink,
        Relink
    }
}
