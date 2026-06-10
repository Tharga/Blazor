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
