# Tharga.Blazor
[![NuGet](https://img.shields.io/nuget/v/Tharga.Blazor)](https://www.nuget.org/packages/Tharga.Blazor)
![Nuget](https://img.shields.io/nuget/dt/Tharga.Blazor)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Generic reusable Blazor UI components. Works with both **Blazor Server** and **Blazor WebAssembly**. Built on [Radzen.Blazor](https://blazor.radzen.com/).

Documentation: [blazor.tharga.net](https://blazor.tharga.net)

## Installation

```bash
dotnet add package Tharga.Blazor
```

## Setup

Register services in `Program.cs`:

```csharp
builder.Services.AddThargaBlazor(o => o.Title = "My Application");
```

You can also bind configuration from `appsettings.json`:

```csharp
builder.Services.AddThargaBlazor(configuration: builder.Configuration);
```

Or combine both (code overrides config):

```csharp
builder.Services.AddThargaBlazor(o => o.Title = "My App", configuration: builder.Configuration);
```

Configuration section in `appsettings.json`:

```json
{
  "Tharga": {
    "Blazor": {
      "Title": "Application Name"
    }
  }
}
```

This registers:
- `BreadCrumbService` (scoped)
- `ILocalStorageService` from Blazored.LocalStorage — **deprecated**, see below
- `IOptions<BlazorOptions>` for configuration
- `IBlazorLanguageProvider` and `LanguageResolver` (scoped), both with `TryAdd`

The `ILocalStorageService` registration is deprecated and will be removed in a future major version. No Tharga.Blazor component uses it: `ExpandableCard` reaches the browser's local storage directly. Blazored.LocalStorage is archived upstream. If your application injects `ILocalStorageService`, reference Blazored.LocalStorage and call `AddBlazoredLocalStorage()` yourself, so that nothing changes for you when the registration goes.

Radzen is not registered for you. Add `builder.Services.AddRadzenComponents();` and render `<RadzenComponents />` in your layout, as the [getting started guide](https://blazor.tharga.net/articles/getting-started.html) shows.

## Components

### Buttons

Pre-built button components with busy states and error handling.

- **`ActionButton`** — Wraps async click actions with automatic loading state, error notifications, and optional delay.
- **`StandardButton`** — Base button with typed styling (`Normal`, `Discrete`, `Information`, `Warning`, `Error`, `Confirm`, `Reject`).
- **`CancelButton`** — Pre-styled cancel button.
- **`CopyButton`** — Copies text to clipboard via JS interop with success notification.

### Breadcrumbs

Route-aware breadcrumb trail with programmatic control.

```razor
<BreadCrumbs />
```

Use `BreadCrumbService` to customize the trail:

```csharp
@inject BreadCrumbService BreadCrumbService

// Add a virtual segment
BreadCrumbService.AddVirtualSegment("Details", "/items/42");

// Convert a query parameter into a breadcrumb
BreadCrumbService.RegisterVirtualSegmentQueryParam("category");

// Relink, unlink or remove segments
BreadCrumbService.RelinkSegment("Items", "/items?status=active");
BreadCrumbService.UnlinkSegment("Current");
BreadCrumbService.RemoveSegment("Records");

// Show a readable name for an id segment, keeping its position and link
BreadCrumbService.SetSegmentText(caseId, "KS 2026-14");

// Re-render the trail, e.g. after a language change
BreadCrumbService.Refresh();
```

Register an `IBreadCrumbTextProvider` to translate URL segments app-wide (`cases` → `Ärenden`). It is asked every time the trail is read; returning `null` keeps the default capitalised text. See the [breadcrumbs article](https://blazor.tharga.net/articles/breadcrumbs.html) for details.

### Error Handling

```razor
<CustomErrorBoundary>
    <ChildContent>@Body</ChildContent>
</CustomErrorBoundary>
```

Catches unhandled exceptions, logs them with a correlation ID, and displays an error view with recovery.

An `UnauthorizedAccessException` is treated as an expected outcome rather than a crash: it renders a compact **Access denied** panel carrying the exception message, and logs at `Warning`. Everything else renders the crash panel and logs at `Error`.

Exception messages and stack traces are shown **only** when the host opts in, so they never reach a production user:

```csharp
builder.Services.AddThargaBlazor(o => o.ShowExceptionDetails = builder.Environment.IsDevelopment());
```

### Layout & Display

- **`ExpandableCard`** — Collapsible card with an optional leading icon or image, header menu, and local storage state persistence (`AllowSaveState`, keyed by `StateKey`). Needs no service registration of its own.
- **`Loading`** — Indeterminate progress indicator (centered or inline).
- **`Title`** — Dynamic page title based on route and `BlazorOptions.Title`.
- **`DateTimeView`** — Date/time display: relative, absolute, or the day alone, with whatever the text leaves out as the tooltip. Renders in English or Swedish, and can recompute a relative label on an interval.
- **`TimeSpanView`** — Formatted time span display, in English or Swedish.

### Language

`DateTimeView` and `TimeSpanView` resolve their language from, in order: the `Language` parameter, an `IBlazorLanguageProvider` the host registered, `BlazorOptions.Language`, and `CultureInfo.CurrentUICulture` — falling back to English for a culture the Toolkit has no wording for.

```csharp
builder.Services.AddThargaBlazor(o => o.Language = Language.Sv);
```

Options are a singleton, so a multi-tenant site resolves language per circuit instead:

```csharp
builder.Services.AddScoped<IBlazorLanguageProvider, TenantLanguageProvider>();
```

A relative label describes a moving quantity but only recomputes when the component renders, so in a static grid it goes stale. `Display="EDateTimeDisplay.Absolute"` (or `Date`, for narrow columns) avoids the problem entirely, and `RefreshInterval` opts into recomputing it. See [Date and duration views](https://blazor.tharga.net/articles/datetimeview.html).

## Upgrading

### To 2.4: Radzen.Blazor 12

Tharga.Blazor 2.4 depends on Radzen.Blazor 12. Nothing in Tharga.Blazor's own API changed: every Radzen type it exposes (`Variant`, `ButtonSize`, `ButtonStyle`, `Shade`, `ProgressBarCircularSize`) and every Radzen component it renders has the same members in 12 as in 11, and no new registration, script or stylesheet is needed.

Two things can still reach your application:

- **A direct Radzen.Blazor reference below 12** now fails restore with `NU1605` (package downgrade). Raise it to 12.x, or remove it and use the version that comes with Tharga.Blazor.
- **Radzen 12 changed some defaults** in components you may use directly: `RadzenDatePicker` navigates by drilling down instead of month and year drop-downs, and `RadzenChart` value axes, tick steps and category labels are laid out differently. Each change has a parameter that restores the old behaviour. See the [Radzen.Blazor 12.0.0 release notes](https://github.com/radzenhq/radzen-blazor/releases/tag/v12.0.0).

`ExpandableCard` no longer uses Blazored.LocalStorage. It keeps the same storage keys and value format, so a card's remembered state survives the upgrade.

## Target Frameworks

- .NET 10

## Dependencies

- [Radzen.Blazor](https://blazor.radzen.com/) 12
- [Blazored.LocalStorage](https://github.com/Blazored/LocalStorage), only for the deprecated `ILocalStorageService` registration
- [Tharga.Toolkit](https://www.nuget.org/packages/Tharga.Toolkit)

## Build & Test

```bash
dotnet build -c Release
dotnet test -c Release
```

## License

MIT
