# Upgrading

What changes for an application when it moves to a new Tharga.Blazor version, and what, if
anything, it has to do.

## 2.4: Radzen.Blazor 12

Tharga.Blazor 2.4 moves from Radzen.Blazor 11 to Radzen.Blazor 12.

### Tharga.Blazor's own API is unchanged

Every Radzen type that appears in Tharga.Blazor's parameters (`Variant`, `ButtonSize`,
`ButtonStyle`, `Shade` and `ProgressBarCircularSize`) has the same values in Radzen 12, and every
Radzen component Tharga.Blazor renders has the same parameters. Radzen 12 needs no new service
registration, script or stylesheet: `AddRadzenComponents()`, `<RadzenComponents />` and the theme
references stay as they are.

### What can still reach your application

- **A direct Radzen.Blazor reference below 12.** Restore fails with `NU1605` (detected package
  downgrade), because Tharga.Blazor now needs 12. Raise your reference to 12.x, or remove it and use
  the version Tharga.Blazor brings.
- **Changed defaults in Radzen components you use yourself.** Radzen 12 changed behaviour, not
  signatures, so these compile unchanged but can look different:
  - `RadzenDatePicker` navigates by drilling down from the calendar title instead of showing month
    and year drop-downs. `NavigationMode="DatePickerNavigationMode.DropDown"` restores the old look.
  - `RadzenChart` column, bar and area series include zero on the value axis, line series with only
    negative values fit their data, value axes choose their tick step from the data, and an axis
    that sets only `Min` or only `Max` rounds the other end to a tick. `Range` and `Step` on
    `RadzenValueAxis`, or setting both `Min` and `Max`, restore the old axes.
  - `RadzenChart` category labels that do not fit are rotated, skipped or shortened.
    `LabelFit="CategoryAxisLabelFit.None"` on `RadzenCategoryAxis` restores the old labels.
  - `RadzenDataGrid` saves enum filter values in its settings as numbers. Settings saved before
    still load.
  - Applications published with trimming see analyzer warnings where they call Radzen's dynamic
    LINQ API directly.

  The full list is in the
  [Radzen.Blazor 12.0.0 release notes](https://github.com/radzenhq/radzen-blazor/releases/tag/v12.0.0).

### Blazored.LocalStorage

`ExpandableCard` no longer uses Blazored.LocalStorage: it calls the browser's `localStorage`
directly. It reads and writes the same keys, `ExpandableCard.{key}.Selected`, in the same format, so
the state a user left a card in survives the upgrade.

`AddThargaBlazor` still registers Blazored's `ILocalStorageService`, so an application that injects
it keeps working. That registration is deprecated and will be removed in a future major version,
together with the package reference. If your application injects `ILocalStorageService`, add a
reference to Blazored.LocalStorage and call `AddBlazoredLocalStorage()` yourself now; registering it
twice is harmless.
