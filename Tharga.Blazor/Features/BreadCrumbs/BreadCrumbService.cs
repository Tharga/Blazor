using Microsoft.AspNetCore.Components;

namespace Tharga.Blazor.Features.BreadCrumbs;

public class BreadCrumbService
{
    private BreadCrumb[] _segments = [];
    //private Func<IEnumerable<BreadCrumb>, IEnumerable<BreadCrumb>> _modifier;
    private readonly Dictionary<string, List<Modifier>> _modifiers = new ();
    private readonly NavigationManager _navigationManager;

    public BreadCrumbService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        navigationManager.LocationChanged += (s, _) => { Build(navigationManager, s); };

        Build(navigationManager, this);
    }

    private void Build(NavigationManager navigationManager, object s)
    {
        var parts = navigationManager.Uri
            .Split('/')
            .Skip(3)
            .ToArray();

        _segments = parts
            .Where(x => !string.IsNullOrEmpty(x))
            .Select((x, i) =>
            {
                var path = string.Join("/", parts.Take(i + 1));
                return new BreadCrumb { Text = x, Path = path };
            })
            .ToArray();

        if (_modifiers.TryGetValue(navigationManager.Uri, out var modifiers))
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
                        case null:
                            return x;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                })
                .Where(x => x != null)
                .ToArray();
        }

        //_segments = _modifier?.Invoke(_segments).ToArray() ?? _segments;
        ChangeEvent?.Invoke(s, EventArgs.Empty);
    }

    public event EventHandler<EventArgs> ChangeEvent;

    public IEnumerable<BreadCrumb> BreadCrumbItems =>
        _segments
            .Select((path, index) =>
            {
                var text = path.Text.Substring(0, 1).ToUpper() + path.Text.Substring(1);
                var disabled = index == _segments.Length - 1;

                return new BreadCrumb
                {
                    Text = text,
                    Path = disabled ? null : path.Path
                };
            });

    //public void Update(Func<IEnumerable<BreadCrumb>, IEnumerable<BreadCrumb>> modifier)
    //{
    //    _modifier = modifier;
    //    _segments = modifier.Invoke(_segments).ToArray();
    //    ChangeEvent?.Invoke(this, EventArgs.Empty);
    //}

    public void UnlinkSegment(string text)
    {
        if (_modifiers.TryGetValue(_navigationManager.Uri, out var modifiers))
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
            _modifiers.Add(_navigationManager.Uri, [new Modifier { Text = text, Modifyer = Modifyer.Unlink}]);
            Build(_navigationManager, this);
        }
    }

    public void RemoveSegment(string text)
    {
        if (_modifiers.TryGetValue(_navigationManager.Uri, out var modifiers))
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
            _modifiers.Add(_navigationManager.Uri, [new Modifier { Text = text, Modifyer = Modifyer.Remove}]);
            Build(_navigationManager, this);
        }
    }

    record Modifier
    {
        public required string Text { get; init; }
        public required Modifyer Modifyer { get; init; }
    }

    public enum Modifyer
    {
        Remove,
        Unlink
    }
}