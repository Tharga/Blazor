# Tharga.Blazor
[![NuGet](https://img.shields.io/nuget/v/Tharga.Blazor)](https://www.nuget.org/packages/Tharga.Blazor)
![Nuget](https://img.shields.io/nuget/dt/Tharga.Blazor)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Generic reusable Blazor UI components. Works with both **Blazor Server** and **Blazor WebAssembly**. Built on [Radzen.Blazor](https://blazor.radzen.com/).

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
- `ILocalStorageService` from Blazored.LocalStorage
- `IOptions<BlazorOptions>` for configuration

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

// Relink or unlink segments
BreadCrumbService.RelinkSegment("Items", "/items?status=active");
BreadCrumbService.UnlinkSegment("Current");
```

### Error Handling

```razor
<CustomErrorBoundary>
    <ChildContent>@Body</ChildContent>
</CustomErrorBoundary>
```

Catches unhandled exceptions, logs them with a correlation ID, and displays an error view with recovery.

### Layout & Display

- **`ExpandableCard`** — Collapsible card with optional icon, header menu, and local storage state persistence.
- **`Loading`** — Indeterminate progress indicator (centered or inline).
- **`Title`** — Dynamic page title based on route and `BlazorOptions.Title`.
- **`DateTimeView`** — Formatted date/time display with relative duration tooltip.
- **`TimeSpanView`** — Formatted time span display.

## Target Frameworks

- .NET 9
- .NET 10

## Dependencies

- [Radzen.Blazor](https://blazor.radzen.com/)
- [Blazored.LocalStorage](https://github.com/Blazored/LocalStorage)
- [Tharga.Toolkit](https://www.nuget.org/packages/Tharga.Toolkit)

## Build & Test

```bash
dotnet build -c Release
dotnet test -c Release
```

## License

MIT
