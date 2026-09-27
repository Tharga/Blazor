# Feature: breadcrumb-segment-text

## Goal
Let a host change the text of a breadcrumb segment in place and translate segments app-wide, and fix the
defects found in the breadcrumb and title components on the way.
Source: [Tharga/Blazor#27](https://github.com/Tharga/Blazor/issues/27) plus backlog items 1 and 2.

## Scope
- **Bug (#27):** the modifier add-check in `RemoveSegment`/`UnlinkSegment`/`RelinkSegment` is case-sensitive
  while `Build` matches case-insensitively with `SingleOrDefault`, so `RemoveSegment("records")` followed by
  `UnlinkSegment("Records")` throws `InvalidOperationException`. Make both case-insensitive.
- **`SetSegmentText(string segment, string text)`** — scoped to the current URL like the other modifiers; keeps
  position and link; combinable with `UnlinkSegment`/`RelinkSegment` (a segment holds a text override plus at
  most one link/remove modifier).
- **`IBreadCrumbTextProvider`** — optional host service, `string GetText(string segment, string path)`,
  evaluated every time `BreadCrumbItems` is read. Null falls back to today's capitalised URL text. Resolved via
  `IServiceProvider.GetService<T>()` so hosts that never register one (or never call `AddThargaBlazor`) are
  unaffected.
- **`Refresh()`** — raises `ChangeEvent` so crumbs re-render after a language change without navigating.
- **Leaked handlers (backlog #1):** `BreadCrumbs.razor` and `Title.razor` unsubscribe on dispose.
- **Cleanup (backlog #2):** redundant default initializers and redundant usings in both components.

## Out of scope
- `DateTimeView` date-only form (#26) — separate feature.

## Acceptance criteria
- The case-mixed modifier sequence from #27 no longer throws (test reproduces the old failure).
- `SetSegmentText` changes text only; position and link are preserved; works together with unlink and relink.
- An explicit `SetSegmentText` wins over the text provider; the provider wins over the URL text; null from the
  provider falls back.
- `Refresh()` raises `ChangeEvent`.
- Disposing `BreadCrumbs` and `Title` removes their handlers (verified by test).
- Existing behaviour and markup unchanged when none of the new API is used; full suite green.
- README and `docs/articles/breadcrumbs.md` document the new API.

## Done condition
All criteria met, user confirms, close-out per shared instructions (records closed, #27 commented and closed).
