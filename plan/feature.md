# Feature: Localize DateTimeView / TimeSpanView and stop the relative label going stale

## Goal

Let `DateTimeView` and `TimeSpanView` render in Swedish as well as English, and give
`DateTimeView` a way to stop showing a relative label that was computed once and never
recomputed. Both without changing what any existing caller renders.

## Why

Two open records, both against the same line of the same 6-line component:

- **[GitHub #24](https://github.com/Tharga/Blazor/issues/24)** — both components call the Toolkit
  formatter with no options, so it falls back to a hard-coded `"en"`. Swedish wording already
  exists in `Tharga.Toolkit.Standard`; these two components are the one place it cannot be
  reached. Reported by an external consumer running multi-tenant sites where language is chosen
  per tenant, not per thread culture.
- **`$DOC_ROOT/Tharga/Requests.md` → Tharga.Blazor**, filed 2026-09-09 by **Tharga.Team** — the
  relative label never refreshes, so an audit grid renders two entries written in the same
  millisecond as "39 seconds ago" and "34 seconds ago". For an audit log that is a false
  statement about history.

## Scope

In:

- A `Language` parameter on both components.
- A resolution chain: parameter → `IBlazorLanguageProvider` (scoped, host-implemented) →
  `BlazorOptions.Language` → `CultureInfo.CurrentUICulture` → English.
- `DateTimeView.Display` (`Relative` | `Absolute`) and `DateTimeView.RefreshInterval`, both
  opt-in.
- Tests for both components — neither has any today.
- A docs article, README updates, and closing both records.

Out:

- Any change to `Tharga.Toolkit`. The resolution is done on the Blazor side so the throwing
  `Get(string culture)` overloads are never called.
- Localizing the library's other hard-coded English strings ("Cancel", "Loading", "Access
  denied", …). Worth its own feature; not this one.
- The pre-existing event-handler leaks in `Title.razor` and `BreadCrumbs.razor`, noted in the
  backlog while passing.

## Acceptance criteria

1. `<DateTimeView Date="@x" />` with no parameters renders **byte-identical markup to 2.3.3** —
   same text, same `title` — including when no services are registered at all, for every culture
   the Toolkit has no wording for. A host already running under `sv-SE` now gets Swedish without
   changing a line, which is the point of #24 rather than a regression.
2. `<DateTimeView Date="@x" Language="Language.Sv" />` renders Swedish wording.
3. A host registering its own `IBlazorLanguageProvider` gets that language at every call site
   with no markup changes, and it works inside a Radzen dialog.
4. An app whose `CurrentUICulture` is neither `en` nor `sv` — or is the invariant culture —
   renders English and **does not throw**.
5. `Display="EDateTimeDisplay.Absolute"` puts the timestamp in the text and the relative form in
   the tooltip.
6. `RefreshInterval` unset starts no timer. Set, the label recomputes and the timer is disposed
   with the component.
7. Full suite green; build warnings stay at or under 15.

## Done condition

All acceptance criteria met, docs updated on both surfaces, GitHub #24 commented and closed,
the `Requests.md` item marked Done with a follow-up entry for Tharga.Team, and the PR opened
against `master`.
