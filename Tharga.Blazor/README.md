# Tharga Blazor
[![NuGet](https://img.shields.io/nuget/v/Tharga.Blazor)](https://www.nuget.org/packages/Tharga.Blazor)
![Nuget](https://img.shields.io/nuget/dt/Tharga.Blazor)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Drop-in Blazor UI components for common application needs — buttons with busy states, breadcrumb navigation, error boundaries, expandable cards, and more. Works with both **Blazor Server** and **Blazor WebAssembly**.

Built on [Radzen.Blazor](https://blazor.radzen.com/). Targets **.NET 9** and **.NET 10**.

## Quick Start

```bash
dotnet add package Tharga.Blazor
```

```csharp
builder.Services.AddThargaBlazor(o => o.Title = "My Application");
```

## What's Included

- **Buttons** — `ActionButton`, `CancelButton`, `CopyButton`, `StandardButton` with built-in busy states, error handling, and typed styling.
- **Breadcrumbs** — Route-aware breadcrumb trail with virtual segments, query parameter segments, and programmatic control.
- **Error Handling** — `CustomErrorBoundary` with correlation ID logging and recovery.
- **Layout** — `ExpandableCard`, `Loading`, `Title`, `DateTimeView`, `TimeSpanView`.

For full documentation and usage examples, see the [GitHub README](https://github.com/Tharga/Toolkit-Blazor).
