# Tharga Blazor
[![NuGet](https://img.shields.io/nuget/v/Tharga.Blazor)](https://www.nuget.org/packages/Tharga.Blazor)
![Nuget](https://img.shields.io/nuget/dt/Tharga.Blazor)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Generic reusable Blazor UI components. Works with both **Blazor Server** and **Blazor WebAssembly**. Built on [Radzen.Blazor](https://blazor.radzen.com/).

No team or authentication dependencies — this package is for UI components only.

## Components

- **Buttons** - `ActionButton`, `CancelButton`, `CopyButton`, `StandardButton` with built-in busy states and error handling.
- **Breadcrumbs** - `BreadCrumbService` with virtual segments, query parameter segments, and segment relinking.
- **Error handling** - `CustomErrorBoundary` with correlation ID logging.
- **Layout** - `ExpandableCard`, `Loading`, `Title`, `DateTimeView`, `TimeSpanView`.

## Related packages

| Package | Description |
|---------|-------------|
| [Tharga.Team.Blazor](https://www.nuget.org/packages/Tharga.Team.Blazor) | Team management components (depends on this package) |
| [Tharga.Team](https://www.nuget.org/packages/Tharga.Team) | Domain models and authorization primitives |
