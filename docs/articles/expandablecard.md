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

`Selected` controls whether the card starts expanded (default collapsed). Clicking the title bar toggles it thereafter.

```razor
<ExpandableCard Text="Open by default" Icon="info" Selected="true">
    ...
</ExpandableCard>
```

## Parameters

| Parameter | Type | Description |
|---|---|---|
| `Text` | `string` | Title text shown in the title bar. |
| `Icon` | `string` | Radzen material-icon name for the leading slot. Ignored when `ImageUrl` is set. |
| `ImageUrl` | `string` | Image URL for the leading slot; takes precedence over `Icon`. Height is capped to the icon size, aspect ratio preserved. |
| `TopMenu` | `RenderFragment` | Content rendered at the right edge of the title bar. |
| `ChildContent` | `RenderFragment` | Body content, shown when the card is expanded. |
| `Selected` | `bool` | Whether the card starts expanded. Default `false`. |
