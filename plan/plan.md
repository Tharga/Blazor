# Plan: breadcrumb-segment-text

## Steps
- [x] 1. NuGet update — Radzen.Blazor 11.3.2 → 11.5.0, bunit 2.11.3, Microsoft.NET.Test.Sdk 18.10.1, Moq 4.21.0,
      xunit.v3 4.0.1. No majors. 102/102 tests pass. Commit `1daf201`.
- [x] 2. Fix case-sensitive modifier add-check (#27 bug) — test `Modifiers_DifferingOnlyInCase_DoNotThrow` reproduced
      the `InvalidOperationException`; add-check now `InvariantCultureIgnoreCase` like `Build`. First modifier still wins.
- [x] 3. `SetSegmentText` — modifiers are now one entry per segment (case-insensitive dictionary per URL) holding an
      optional text plus an optional link/remove modifier. Link modifiers keep first-wins; text is last-wins and
      null restores the default. Explicit text is shown as given (not capitalised). Event fires only on change.
- [x] 4. `IBreadCrumbTextProvider` — **deviation:** constructor injection via a second
      `BreadCrumbService(NavigationManager, IBreadCrumbTextProvider)` ctor instead of `GetService`. The service is
      constructed by DI, so MS DI picks the two-arg ctor only when a provider is registered; the one-arg ctor is
      kept for binary compatibility. Asked on every read, only for URL segments (not virtual ones — their text
      already comes from the host). Precedence: explicit text → provider → capitalised URL text.
- [x] 5. `Refresh()` — raises `ChangeEvent`; no rebuild needed since text is resolved on read.
      Steps 3–5: 17 new tests, 120/120 pass.
- [~] 6. `BreadCrumbs.razor` and `Title.razor`: unsubscribe on dispose; remove redundant initializers/usings
- [ ] 7. Full test suite green; commit
- [ ] 8. Docs: README + `docs/articles/breadcrumbs.md` (at close-out)

## Notes
- Branch `feature/breadcrumb-segment-text` from `master` (GitHub Actions → PR to master).

## Last session
2026-09-27 — branch created, dependencies updated, plan written. Next: step 2.

## README changes needed at completion
- Breadcrumbs section: `SetSegmentText`, `IBreadCrumbTextProvider`, `Refresh()`.
