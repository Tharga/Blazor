using Microsoft.AspNetCore.Components;

namespace Tharga.Blazor.Features.BreadCrumbs;

public class BreadCrumbService
{
    private BreadCrumb[] _segments = [];
    private Func<IEnumerable<BreadCrumb>, IEnumerable<BreadCrumb>> _modifier;

    public BreadCrumbService(NavigationManager navigationManager)
    {
        navigationManager.LocationChanged += (s, _) =>
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

            _segments = _modifier?.Invoke(_segments).ToArray() ?? _segments;
            ChangeEvent?.Invoke(s, EventArgs.Empty);
        };
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

    public void Update(Func<IEnumerable<BreadCrumb>, IEnumerable<BreadCrumb>> modifier)
    {
        _modifier = modifier;
        _segments = modifier.Invoke(_segments).ToArray();
        ChangeEvent?.Invoke(this, EventArgs.Empty);
    }
}