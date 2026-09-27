# Feature: datetimeview-date-display

## Goal
Give `DateTimeView` a day-only form, and make its tooltip carry whatever the text does not show, so consumers
stop copying the component's formatting and language resolution into their own wrappers.
Source: [Tharga/Blazor#26](https://github.com/Tharga/Blazor/issues/26), plus backlog item 3 (leftover test packages).

## Scope
- `EDateTimeDisplay.Date` — appended after `Absolute` so existing numeric values are unchanged. Text is
  `Date.ToLocalDateString()` (Tharga.Toolkit, `yyyy-MM-dd`, local time).
- Tooltip = every form the text omits, joined with ` · `:
  - `Relative` → absolute timestamp (unchanged)
  - `Absolute` → relative (unchanged)
  - `Date` → `2026-09-12 09:13:14 · 4 days ago`
- `RefreshInterval` is ignored for `Date`, as for `Absolute` — the text cannot go stale.
- Drop `Microsoft.NET.Test.Sdk` and `xunit.runner.visualstudio` from the test project.

## Acceptance criteria
- `Display="EDateTimeDisplay.Date"` renders the local date as text and `timestamp · relative` as tooltip.
- The relative part of the tooltip follows language resolution (Swedish test).
- Existing `Relative` and `Absolute` output unchanged; null date still renders an empty span.
- No refresh timer runs for `Date`.
- Tests still discovered and run after removing the two packages (count unchanged, not zero).
- README and `docs/articles/datetimeview.md` document the new form.

## Done condition
All criteria met, user confirms, close-out per shared instructions (records closed, #26 commented and closed).
