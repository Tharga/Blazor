# ExpandableCard

`ExpandableCard` is a collapsible [Radzen](https://blazor.radzen.com/) card with a clickable title bar. The title bar carries a leading visual (an icon **or** an image), the title text, and an optional right-aligned menu slot. Clicking the title bar expands or collapses the body.

## Basic usage

```razor
<ExpandableCard Text="Team settings" Icon="settings">
    <p>Content shown when the card is expanded.</p>
</ExpandableCard>
```

## Leading icon or image

The slot to the left of the title text shows **either** a font icon **or** an image — never both. Set `Icon` for a Radzen material-icon name, or `ImageUrl` for an image (for example a team avatar or a logo served from a URL).

```razor
@* Font icon *@
<ExpandableCard Text="Team settings" Icon="group" />

@* Image — e.g. a team avatar *@
<ExpandableCard Text="Contoso" ImageUrl="https://cdn.example.com/teams/contoso.png" />
```

When both `Icon` and `ImageUrl` are set, **the image wins** and `Icon` is ignored. The image height is capped to the icon size (`--rz-icon-size`) and its width scales to preserve aspect ratio, so even a large source image renders at the same height an icon would.

The image's `alt` text defaults to `Text`. There is no built-in fallback for a missing or failed image URL — supply `ImageUrl` only when an image actually exists, and fall back to `Icon` (or neither) otherwise.

## Title-bar menu

`TopMenu` renders content at the right edge of the title bar — for actions that belong with the card header:

```razor
<ExpandableCard Text="Report" Icon="description">
    <TopMenu>
        <CopyButton Content="@reportId" Size="ButtonSize.ExtraSmall" />
    </TopMenu>
    <ChildContent>
        @* body *@
    </ChildContent>
</ExpandableCard>
```

## Initial expanded state

`Selected` controls whether the card starts expanded (default collapsed). Clicking the title bar toggles it thereafter. Once the user has expanded or collapsed the card themselves, their choice is remembered and takes precedence over `Selected` — see [Remembering the state](#remembering-the-state).

```razor
<ExpandableCard Text="Open by default" Icon="info" Selected="true">
    ...
</ExpandableCard>
```

## Remembering the state

`AllowSaveState` is `true` by default: when the user expands or collapses a card, the new state is
written to browser local storage and restored the next time the card renders. `Selected` then acts as
the value used only until the user has expressed a preference.

```razor
@* Remembers whether the user left it open *@
<ExpandableCard Text="Team settings" Icon="settings" StateKey="team.settings">
    ...
</ExpandableCard>

@* Always starts from Selected, never touches storage *@
<ExpandableCard Text="Preview" Icon="visibility" AllowSaveState="false" Selected="true">
    ...
</ExpandableCard>
```

### Give it a `StateKey`

The stored value is keyed as `ExpandableCard.{key}.Selected`, where the key is `StateKey` if you set
one and `Text` otherwise. **Prefer setting `StateKey`.** `Text` is display text, which brings three
problems that a key of your own avoids:

- it is **translated**, so switching UI language would look up a different entry;
- it is **not unique**, so two cards titled the same on different pages would share one state;
- it may be **assigned after the first render**, if the title comes from an async localization or
  content service.

```razor
<ExpandableCard Text="@_secrecyClassesTitle" StateKey="caseManagement.secrecyClasses" Icon="shield">
    ...
</ExpandableCard>
```

The key is captured the first time the card needs it and does not change afterwards, so a `Text` that
resolves late cannot separate the key that was read from the key that gets written. That makes the
fallback safe, but it does not make it *stable* — a `StateKey` is still the right answer whenever the
title is translated or repeated.

### Rendering and interactivity

Persistence needs an interactive renderer, because local storage is reached through JavaScript
interop. Under prerendering or static server rendering the card renders at `Selected` and storage is
left alone.

On an interactive renderer the stored value is read **before the card's first render**, so the card
paints once, already in the state the user left it in — it never paints at its default and then
corrects itself. The cost is that the card renders nothing at all while that read is in flight. On the
initial load of an interactive circuit this is invisible, because Blazor waits for the component
hierarchy to settle before showing the page; navigating to a page later in the same session, the card
appears a moment after the rest of the page rather than flickering.

If local storage is unavailable — blocked by browser policy, for instance — the card falls back to
`Selected` and keeps working. It does not fail to render.

## Parameters

| Parameter | Type | Description |
|---|---|---|
| `Text` | `string` | Title text shown in the title bar. |
| `Icon` | `string` | Radzen material-icon name for the leading slot. Ignored when `ImageUrl` is set. |
| `ImageUrl` | `string` | Image URL for the leading slot; takes precedence over `Icon`. Height is capped to the icon size, aspect ratio preserved. |
| `TopMenu` | `RenderFragment` | Content rendered at the right edge of the title bar. |
| `ChildContent` | `RenderFragment` | Body content, shown when the card is expanded. |
| `Selected` | `bool` | Whether the card starts expanded, until the user's own choice has been stored. Default `false`. |
| `AllowSaveState` | `bool` | Whether the expanded state is persisted to local storage and restored. Default `true`. |
| `StateKey` | `string` | Stable key the state is stored under, as `ExpandableCard.{StateKey}.Selected`. Falls back to `Text` when unset. |
