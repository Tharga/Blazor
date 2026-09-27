# Plan: breadcrumb-segment-text

## Steps
- [x] 1. NuGet update — Radzen.Blazor 11.3.2 → 11.5.0, bunit 2.11.3, Microsoft.NET.Test.Sdk 18.10.1, Moq 4.21.0,
      xunit.v3 4.0.1. No majors. 102/102 tests pass. Commit `1daf201`.
- [x] 2. Fix case-sensitive modifier add-check (#27 bug) — test `Modifiers_DifferingOnlyInCase_DoNotThrow` reproduced
      the `InvalidOperationException`; add-check now `InvariantCultureIgnoreCase` like `Build`. First modifier still wins.
- [~] 3. `SetSegmentText` — restructure modifiers so a segment can carry a text override plus one link/remove modifier
- [ ] 4. `IBreadCrumbTextProvider` — resolved via `GetService`, applied in `BreadCrumbItems`, precedence: explicit text → provider → URL text
- [ ] 5. `Refresh()`
- [ ] 6. `BreadCrumbs.razor` and `Title.razor`: unsubscribe on dispose; remove redundant initializers/usings
- [ ] 7. Full test suite green; commit
- [ ] 8. Docs: README + `docs/articles/breadcrumbs.md` (at close-out)

## Notes
- Branch `feature/breadcrumb-segment-text` from `master` (GitHub Actions → PR to master).

## Last session
2026-09-27 — branch created, dependencies updated, plan written. Next: step 2.

## README changes needed at completion
- Breadcrumbs section: `SetSegmentText`, `IBreadCrumbTextProvider`, `Refresh()`.
