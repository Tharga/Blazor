# Getting started

## Install

```
dotnet add package Tharga.Blazor
```

Tharga.Blazor targets **.NET 9** and **.NET 10** and is built on [Radzen.Blazor](https://blazor.radzen.com/). It works with both Blazor Server and Blazor WebAssembly.

## Register services

In `Program.cs`:

```csharp
builder.Services.AddThargaBlazor(o => o.Title = "My Application");
```

Or bind from configuration:

```csharp
builder.Services.AddThargaBlazor(configuration: builder.Configuration);
```

Combine both — code overrides configuration:

```csharp
builder.Services.AddThargaBlazor(o => o.Title = "My App", configuration: builder.Configuration);
```

With the configuration overload, settings come from the `Tharga:Blazor` section in `appsettings.json`:

```json
{
  "Tharga": {
    "Blazor": {
      "Title": "Application Name",
      "ShowExceptionDetails": false
    }
  }
}
```

`ShowExceptionDetails` lets `CustomErrorBoundary` render exception messages and stack traces. It is off unless set — turn it on in `appsettings.Development.json` only, or from code with `o.ShowExceptionDetails = builder.Environment.IsDevelopment()`. See [Error boundary](errorboundary.md).

`AddThargaBlazor` registers:

- `BreadCrumbService` (scoped)
- `ILocalStorageService` from [Blazored.LocalStorage](https://github.com/Blazored/LocalStorage)
- `IOptions<BlazorOptions>` for configuration

## Add the Radzen prerequisites

Tharga.Blazor components delegate styling and the notification surface to Radzen. Wire Radzen in your root layout so notifications and dialogs work:

```razor
@* App.razor / MainLayout.razor *@
<RadzenComponents />
```

And in `Program.cs`:

```csharp
builder.Services.AddRadzenComponents();
```

## First component

Render the breadcrumb trail anywhere — it reads the current route automatically:

```razor
<BreadCrumbs />
```

Drop a copy button next to an inline value:

```razor
<CopyButton Content="@correlationId" Size="ButtonSize.ExtraSmall" />
```

## Next steps

- [Buttons](buttons.md) — `ActionButton`, `StandardButton`, `CopyButton`, `CancelButton`.
- [Breadcrumbs](breadcrumbs.md) — virtual segments, query-param promotion, relinking.
- [Error boundary](errorboundary.md) — correlation-id logging, access-denied handling, stack-trace opt-in.
