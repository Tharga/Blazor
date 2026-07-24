---
_layout: landing
---

# Tharga.Blazor

Generic reusable Blazor UI components built on [Radzen.Blazor](https://blazor.radzen.com/) — buttons with built-in busy/error handling, route-aware breadcrumbs, a custom error boundary, expandable cards, and small display helpers. Works with both **Blazor Server** and **Blazor WebAssembly**. Targets **.NET 9** and **.NET 10**.

## Package

| Package | What it does |
|---|---|
| [Tharga.Blazor](https://www.nuget.org/packages/Tharga.Blazor) | `ActionButton`, `StandardButton`, `CopyButton`, `CancelButton`, `BreadCrumbs` + `BreadCrumbService`, `CustomErrorBoundary`, `ExpandableCard`, `Loading`, `Title`, `DateTimeView`, `TimeSpanView`. |

## Quick start

```
dotnet add package Tharga.Blazor
```

```csharp
// Program.cs
builder.Services.AddThargaBlazor(o => o.Title = "My App");
```

```razor
@* Anywhere in your app *@
<BreadCrumbs />
<CopyButton Content="@id" Size="ButtonSize.ExtraSmall" />
```

See [Getting started](articles/getting-started.md) for the full setup walkthrough.

## What's in the box

- **Buttons** — `ActionButton` wraps async clicks with busy state + error notifications; `StandardButton` is the typed base (`Normal`, `Discrete`, `Information`, `Warning`, `Error`, `Confirm`, `Reject`); `CopyButton` writes to the clipboard; `CancelButton` is a pre-styled cancel. See [Buttons](articles/buttons.md).
- **Breadcrumbs** — `<BreadCrumbs />` renders a route-aware trail; `BreadCrumbService` adds virtual segments, promotes query parameters, and relinks/unlinks segments. See [Breadcrumbs](articles/breadcrumbs.md).
- **Error handling** — `CustomErrorBoundary` catches unhandled exceptions, logs them with a correlation ID, and renders a recovery surface.
- **Layout & display** — `ExpandableCard` (collapsible, with an optional leading icon or image and local-storage state; see [ExpandableCard](articles/expandablecard.md)), `Loading`, `Title`, `DateTimeView`, `TimeSpanView`.

## Repo

[github.com/Tharga/Blazor](https://github.com/Tharga/Blazor) — source, issues, releases.
