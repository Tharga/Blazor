# Plan: ExpandableCard state persistence (#15, #16)

## Steps

- [x] 1. Verify the reported cause against the source
      Done: persistence is commented out entirely (`ExpandableCard.razor` lines 59, 65, 81), present in
      the original import `1479949`. The issues' "key is the translated `Text`" diagnosis describes the
      commented-out draft, not live behaviour. Recorded in `plan/feature.md`.

- [x] 2. NuGet package check (mandatory feature-start step)
      Done: `dotnet outdated` reports no outdated dependencies across the solution. Nothing to apply.

- [x] 3. Add bUnit to the test project
      Done: bUnit 2.9.0, which targets `net10.0` and carries no xunit dependency, so it composes with
      xunit.v3. `FakeLocalStorageService` added alongside the existing `FakeNavigationManager`; it
      serialises with `System.Text.Json` as Blazored does, records reads and writes, and **yields
      before every operation** so a render-count assertion means something.

- [x] 4. Write the failing tests first (testing rules: no tests exist for this code)
      Done: 15 tests in `ExpandableCardTests.cs`, one per acceptance criterion plus the storage-failure
      and non-interactive-renderer cases.

- [x] 5. Implement the fix in `ExpandableCard.razor`
      Done: `StateKey` parameter, key frozen on first use, key namespaced as
      `ExpandableCard.{key}.Selected`, storage read in `SetParametersAsync` under
      `RendererInfo.IsInteractive`, write on toggle, storage failures degrade to `Selected`.

- [x] 6. Run the full suite in Release
      Done: 39 passed, 0 failed.

- [ ] 7. Docs (`docs:` commit)
      `docs/articles/expandablecard.md`: a state-persistence section, `AllowSaveState` and `StateKey` in
      the parameters table (both missing today). `README.md:100`: make the one-line claim accurate.

- [ ] 8. Commit, push the branch, ask the user to test. Do not open the PR yet.

- [ ] 9. On user confirmation: re-check `dotnet outdated`, close #15 and #16 with evidence, archive
      `plan/feature.md` to the Plan directory, `git rm -r plan`, final `fix:` commit, open the PR.

## Notes and decisions

- **Key is readable, not base64.** The commented-out draft used `Text.ToBase64String()`. Dropped: an
  explicit `StateKey` exists to be recognisable in devtools, and local-storage keys need no escaping.
- **The key freezes on first use.** This is what actually fixes the reporter's case without him setting
  `StateKey` at all: his titles resolve from English fallback to Swedish after the first render, and a
  frozen key keeps read and write on the same string.
- **`AllowSaveState` keeps its `true` default.** Issue #15 proposes exactly this shape (`StateKey`,
  falling back to `Text` "only when it is absent"), and flipping the default would contradict the
  README. Consumer impact is noted in `plan/feature.md` for the release notes.

- **The read happens in `SetParametersAsync`, not `OnInitializedAsync`.** This is a deliberate
  departure from the fix #16 proposes, because that fix does not work. `ComponentBase` renders once
  *before* awaiting an incomplete `OnInitializedAsync` task, so the card still paints at its default
  and still corrects itself afterwards — the flicker moves earlier instead of going away. Measured on
  the real renderer with the same test: reading in `OnInitializedAsync` gives `RenderCount == 10`,
  reading in `SetParametersAsync` before `base.SetParametersAsync` gives `RenderCount == 1`.
  `StoredState_IsAppliedWithoutASecondRender` is the regression guard for exactly this.

  The trade-off, which belongs in the release notes: awaiting before the first render means the card
  renders **nothing** until storage answers, rather than rendering the wrong state. On the initial
  page load of an interactive circuit this is invisible — `Renderer.RenderRootComponentAsync` waits
  for quiescence, so the first paint the user sees already carries the stored state. On a later
  in-app navigation the card appears a beat late instead of flickering. This is issue #16's second
  suggested option ("don't paint until the answer is known"), which the reporter calls the complete
  one.

- **`OnAfterRenderAsync` is gone rather than kept as a fallback.** It would be unreachable:
  `OnAfterRender{Async}` is not invoked during prerendering or static SSR, and every interactive pass
  now reads in `SetParametersAsync`.

## Last session

2026-08-14 — Branch `fix/expandablecard-state-persistence` created off `master`. Steps 1-6 done: the
fix is implemented and the suite is green at 39 tests. Next: step 7, the docs commit.
