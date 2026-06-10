# Buttons

Tharga.Blazor ships four button components. All sit on top of Radzen's `RadzenButton`, so they accept Radzen styling and the `Radzen.ButtonSize` enum.

## StandardButton

The typed base. Pick a `Type` and the variant + style + shade are derived automatically.

```razor
<StandardButton Text="Save"   Type="StandardButton.EButtonType.Confirm" Click="@Save" />
<StandardButton Text="Delete" Type="StandardButton.EButtonType.Error"   Click="@Delete" />
<StandardButton Text="Reset"  Type="StandardButton.EButtonType.Discrete" Click="@Reset" />
```

| `Type` | Style | Variant |
|---|---|---|
| `Normal` | Primary | Filled |
| `Information` | Info | Filled |
| `Warning` | Warning | Filled |
| `Error` / `Reject` | Danger | Filled |
| `Confirm` | Success | Filled |
| `Discrete` | Secondary | Text |

Parameters: `Text`, `Icon`, `Click` (async), `Enabled`, `Visible`, `IsBusy` + `BusyText`, `ToolTip`, `Variant`, `ButtonStyle`, `Shade`, `Style`, `Size` (`Radzen.ButtonSize`).

## CopyButton

Writes `Content` to the system clipboard via JS interop and shows a notification. Empty/whitespace `Content` shows a warning instead.

```razor
<CopyButton Content="@correlationId" />
<CopyButton Content="@correlationId" Size="ButtonSize.ExtraSmall" />
```

### Size parameter

`CopyButton` exposes a `Size` parameter (`Radzen.ButtonSize`: `ExtraSmall` / `Small` / `Medium` / `Large`) defaulting to `Medium`. Use a smaller size when the button is inline next to a short value — e.g. a CorrelationId preview in a grid cell — so the button doesn't dominate the row.

```razor
<RadzenDataGridColumn Title="Correlation">
    <Template Context="row">
        <span>@row.CorrelationId[..8]</span>
        <CopyButton Content="@row.CorrelationId" Size="ButtonSize.ExtraSmall" />
    </Template>
</RadzenDataGridColumn>
```

## ActionButton

Wraps an async `Click` handler with automatic busy state and error notifications. Optional `Delay` keeps the busy state visible long enough to be noticed.

```razor
<ActionButton Text="Save" Click="@SaveAsync" />
```

If `SaveAsync` throws, `ActionButton` catches the exception, shows it via the notification service, and clears the busy flag.

## CancelButton

Pre-styled cancel — `Discrete` type, ready to wire to a dialog close handler or a navigation back action.

```razor
<CancelButton Click="@Cancel" />
```
