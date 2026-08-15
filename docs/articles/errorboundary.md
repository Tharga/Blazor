# Error boundary

`CustomErrorBoundary` is an [`ErrorBoundary`](https://learn.microsoft.com/aspnet/core/blazor/fundamentals/handle-errors) that logs what it catches with a correlation id and shows the user something they can act on. Hosts typically wrap all content in it once, from `MainLayout`, so whatever it decides applies app-wide.

```razor
@* MainLayout.razor *@
<CustomErrorBoundary>
    <ChildContent>@Body</ChildContent>
</CustomErrorBoundary>
```

## What the user sees

The boundary renders one of three ways, in this order.

**1. Your own `ErrorContent`, if you supply it.** It replaces the panel entirely — the boundary adds no heading of its own above it.

```razor
<CustomErrorBoundary>
    <ChildContent>@Body</ChildContent>
    <ErrorContent Context="exception">
        <MyErrorPanel Exception="@exception" />
    </ErrorContent>
</CustomErrorBoundary>
```

**2. An access-denied panel, for `UnauthorizedAccessException`.** A permission failure is an expected outcome, not a crash, so it gets a compact panel carrying the exception's own message — no "Something went wrong!", no stack trace, no correlation id — and is logged at `Warning` rather than `Error`.

```html
<div class="access-denied-content">
    <h3>Access denied</h3>
    <p>You do not have access to this team.</p>
    <button class="btn btn-primary">Try again</button>
</div>
```

Throw `UnauthorizedAccessException` from your authorization layer and this happens app-wide with no per-page code. The message is shown to the user verbatim, so write it for them.

**3. The crash panel, for everything else.** Heading, the correlation id the failure was logged under with a copy button, and a recovery button. Exception details appear only if you allow them — see below.

## Stack traces are off by default

`BlazorOptions.ShowExceptionDetails` controls whether the crash panel renders the exception message and stack trace. **Unset means off**, so a production user is never shown a stack trace.

Turn it on for development from the host environment:

```csharp
builder.Services.AddThargaBlazor(o => o.ShowExceptionDetails = builder.Environment.IsDevelopment());
```

Or per environment, in `appsettings.Development.json`, with the configuration overload:

```json
{
  "Tharga": {
    "Blazor": {
      "ShowExceptionDetails": true
    }
  }
}
```

The setting has no effect on the access-denied panel, which never renders a stack trace.

**Why you have to say.** The library cannot work this out for itself. It holds no ASP.NET Core hosting reference, and neither `IWebHostEnvironment` nor `IHostEnvironment` is registered on Blazor WebAssembly, which uses `IWebAssemblyHostEnvironment` instead. A library that guessed would silently answer "not development" on every WebAssembly host. The host always knows, so the host decides.

## Logging

Every caught exception gets a fresh `CorrelationId`, attached to the exception's `Data` and included in the log scope, so the id on screen matches the entry in your log store.

| Exception | Level |
|---|---|
| `UnauthorizedAccessException` | `Warning` |
| everything else | `Error` |

## Styling

The boundary ships no CSS. It emits `error-content`, `dev-error-details` and `access-denied-content` for you to style — the access-denied class is separate precisely so a permission notice need not look like a failure.

## Parameters

| Parameter | Type | Description |
|---|---|---|
| `ChildContent` | `RenderFragment` | Content the boundary protects. |
| `ErrorContent` | `RenderFragment<Exception>` | Replaces the built-in panels entirely when supplied. |
| `MaximumErrorCount` | `int` | Inherited from `ErrorBoundary`. How many errors are tolerated before the boundary stops recovering. |
