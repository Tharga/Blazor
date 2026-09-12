# Date and duration views

`DateTimeView` renders a moment, `TimeSpanView` renders a length of time. Both are one-line components built on the formatters in `Tharga.Toolkit`, and both render in English or Swedish.

## Basic usage

```razor
<DateTimeView Date="@entry.Timestamp" />
<TimeSpanView TimeSpan="@job.Duration" />
```

`DateTimeView` shows how long ago it was and puts the timestamp in the tooltip. `TimeSpanView` shows the duration and puts the raw value in the tooltip. A null value renders an empty span rather than a placeholder.

## Choosing the language

Language resolves through four steps, and the first with an opinion wins:

1. the `Language` parameter on the component
2. an `IBlazorLanguageProvider` the host registered
3. `BlazorOptions.Language`
4. `CultureInfo.CurrentUICulture`

Anything unresolved renders English. A culture the Toolkit has no wording for — German, or the invariant culture — falls back rather than failing, so a component never takes down a render because of a language it does not know.

The parameter is the direct form:

```razor
<DateTimeView Date="@entry.Timestamp" Language="Language.Sv" />
```

An app that speaks one language sets it once at registration instead:

```csharp
builder.Services.AddThargaBlazor(o => o.Language = Language.Sv);
```

or per environment, under the `Tharga:Blazor` section:

```json
{
  "Tharga": {
    "Blazor": {
      "Language": "Sv"
    }
  }
}
```

## Language per tenant or per user

`BlazorOptions` is resolved as a singleton, so it holds one language for the whole application. A site that serves several tenants, or lets each user pick, needs the language resolved per circuit — register an `IBlazorLanguageProvider`:

```csharp
public class TenantLanguageProvider : IBlazorLanguageProvider
{
    private readonly ITenantContext _tenant;

    public TenantLanguageProvider(ITenantContext tenant) => _tenant = tenant;

    public Language? GetLanguage() => _tenant.Current?.Language;
}
```

```csharp
builder.Services.AddScoped<IBlazorLanguageProvider, TenantLanguageProvider>();
```

Registration is `TryAdd`, so yours wins over the built-in one. Every `DateTimeView` and `TimeSpanView` in the app follows it with no markup changes.

Returning `null` means "no opinion" and falls through to the application default — useful when only some tenants have chosen a language.

**Why a service rather than a cascading value.** A service is resolved wherever the component renders. A cascading value only reaches the render tree it wraps, so a component inside a dialog opened through `DialogService` would silently fall back while the page behind it rendered correctly.

## Absolute or relative

`Display` decides which form is the text; the other becomes the tooltip.

```razor
<DateTimeView Date="@entry.Timestamp" Display="EDateTimeDisplay.Absolute" />
```

| | Text | Tooltip |
|---|---|---|
| `Relative` (default) | `34 seconds ago` | `2026-09-12 07:54:35` |
| `Absolute` | `2026-09-12 07:54:35` | `34 seconds ago` |

**Prefer `Absolute` wherever exact ordering matters.** A relative label is rounded, so two entries written a few hundred milliseconds apart can render as five seconds apart, and a column of them can read as a sequence of events that never happened. For an audit log, a job history, or anything sorted by time, the timestamp is the honest form.

## Keeping a relative label fresh

A relative label describes a moving quantity, but a Blazor component only recomputes when something makes it render. In a grid that loads once and sits there, "34 seconds ago" stays "34 seconds ago".

`RefreshInterval` opts into recomputing it:

```razor
<DateTimeView Date="@entry.Timestamp" RefreshInterval="TimeSpan.FromSeconds(30)" />
```

Unset — the default — starts no timer at all.

Points worth knowing before switching it on:

- **It costs one timer per component instance.** A grid of 50 rows with two date columns is 100 timers, each waking the circuit. Pick an interval matching the granularity you actually show; 30 seconds is plenty for a column measured in minutes.
- **Intervals below one second are raised to one second.** The component shows no finer granularity.
- **It is ignored when `Display` is `Absolute`**, which cannot go stale.
- **No timer starts during prerendering or static server rendering**, only once the component is interactive.

Where a grid already refreshes on a schedule of its own, leave `RefreshInterval` unset and let the grid's own render cycle do the work.

## Parameters

### DateTimeView

| Parameter | Type | Description |
|---|---|---|
| `Date` | `DateTime?` | The moment to describe. Null renders an empty span. |
| `Language` | `Language?` | Overrides the resolved language. Default unset. |
| `Display` | `EDateTimeDisplay` | `Relative` or `Absolute`. Default `Relative`. |
| `RefreshInterval` | `TimeSpan?` | How often to recompute a relative label. Default unset — never recomputes. Minimum one second. |

### TimeSpanView

| Parameter | Type | Description |
|---|---|---|
| `TimeSpan` | `TimeSpan?` | The duration to describe. Null renders an empty span. |
| `Language` | `Language?` | Overrides the resolved language. Default unset. |
