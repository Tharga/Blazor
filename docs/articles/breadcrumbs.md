# Breadcrumbs

`<BreadCrumbs />` renders a breadcrumb trail derived from the current `NavigationManager` route. `BreadCrumbService` (registered scoped by `AddThargaBlazor`) lets you customize the trail for routes whose URL doesn't tell the full story.

## Default render

```razor
<BreadCrumbs />
```

For a route like `/projects/42/details`, this renders **Home › Projects › 42 › Details** with each segment linked back to its prefix path.

## Add a virtual segment

When a UI surface lives "inside" a logical parent that isn't in the URL — for example, a modal-spawned subview — push a virtual breadcrumb manually:

```csharp
@inject BreadCrumbService BreadCrumbService

protected override void OnInitialized()
{
    BreadCrumbService.AddVirtualSegment("Details", "/items/42");
}
```

The virtual segment shows up alongside the route-derived segments until the next navigation.

## Promote a query parameter

Some routes use query parameters as meaningful sub-context (e.g. `?category=invoices`). Register the param and the value becomes a breadcrumb segment:

```csharp
BreadCrumbService.RegisterVirtualSegmentQueryParam("category");
```

Now `/items?category=invoices` renders **Home › Items › invoices**.

## Relink / unlink segments

Override the destination of a generated segment, or strip its link so it renders as plain text:

```csharp
// Override target URL of an existing segment
BreadCrumbService.RelinkSegment("Items", "/items?status=active");

// Render a segment as text (no link)
BreadCrumbService.UnlinkSegment("Current");

// Drop a segment from the trail
BreadCrumbService.RemoveSegment("Records");
```

Segments are matched by their URL text, ignoring case. Each change applies to the current URL only (query string ignored), so it is typically made from the page that owns the route. A segment takes one link change: the first of relink, unlink or remove wins.

## Change a segment's text

Route segments are often ids. `SetSegmentText` shows a readable name instead, while the segment keeps its position and its link:

```csharp
// On /cases/7318ed96/records/6ab8cfeb
BreadCrumbService.SetSegmentText(caseId, "KS 2026-14");
BreadCrumbService.SetSegmentText(recordId, "Handling 4");
```

This renders **Cases › KS 2026-14 › Records › Handling 4**. The text is shown exactly as given, not capitalised. It combines with `RelinkSegment` and `UnlinkSegment`, calling it again replaces the text, and passing `null` restores the default.

## Translate segments

Static segments show their URL text, capitalised (`cases` → **Cases**). To translate them app-wide, register an `IBreadCrumbTextProvider`:

```csharp
public class BreadCrumbTexts : IBreadCrumbTextProvider
{
    private readonly LanguageResolver _language;

    public BreadCrumbTexts(LanguageResolver language) => _language = language;

    public string GetText(string segment, string path)
    {
        if (_language.Resolve() != Language.Sv) return null;

        return segment switch
        {
            "cases" => "Ärenden",
            "records" => "Handlingar",
            _ => null
        };
    }
}

builder.Services.AddScoped<IBreadCrumbTextProvider, BreadCrumbTexts>();
```

- `segment` is the URL text of the segment; `path` is the route up to and including it, without a leading slash (`cases/7318ed96`).
- Return `null` to keep the default capitalised text.
- The provider is asked every time the trail is read, so it follows the current language.
- Text set with `SetSegmentText` wins over the provider.
- It applies to segments from the URL only. Virtual segments and promoted query parameters already carry the text you gave them.

## Refresh after a language change

The trail re-renders on navigation and whenever the service changes. When something else changes what it should show — typically the language — call `Refresh()`:

```csharp
BreadCrumbService.Refresh();
```

## Rendering inside a layout

Drop `<BreadCrumbs />` in your `MainLayout.razor` once and it picks up route changes automatically — no per-page wiring required.

```razor
@* MainLayout.razor *@
<header>
    <BreadCrumbs />
</header>
<main>
    @Body
</main>
```
