# Feature: ExpandableCard state persistence (GitHub #15, #16)

## Goal

Make `ExpandableCard`'s `AllowSaveState` actually persist and restore the expanded state, keyed on a
stable value, and restore it before the first paint so no card flickers on load.

## Background — what the issues got wrong

Both issues were filed against the published package and diagnosed by reflection over
`Tharga.Blazor.dll`. They conclude the storage key is the translated `Text`. The source says otherwise:
in `ExpandableCard.razor` the key, the `GetItemAsync` read and the `SetItemAsync` write are all
**commented out**, and have been since the project was imported (`1479949`). `AllowSaveState` has never
had any effect.

So this is not two bugs in a working feature — it is one unimplemented feature, whose absence produces
both reported symptoms:

- #15 (state never restores) is the direct consequence.
- #16 (flicker) is what the commented-out design *would* have produced, because the only lifecycle
  override is `OnAfterRenderAsync`.

`README.md:100` already documents the persistence as if it exists, so the docs are the promise this
change finally keeps.

## Scope

- Add a `StateKey` parameter: an explicit, stable, non-translated key for the stored state.
- Fall back to `Text` when `StateKey` is absent, and **freeze the resolved key on first use** so a
  `Text` that arrives late (async localization) cannot split the read key from the write key.
- Namespace the stored key as `ExpandableCard.{key}.Selected` so it cannot collide with a host's own
  local-storage entries.
- Restore in `OnInitializedAsync` when `RendererInfo.IsInteractive`, so the card paints once, in its
  stored state. Keep the `OnAfterRenderAsync` read as a fallback for renderers that report
  non-interactive at init.
- Add bUnit to the test project (no component tests exist today) and cover both issues.
- Update `docs/articles/expandablecard.md` and `README.md`.

## Out of scope

- Base64-encoding the key (the commented-out draft did this). An opaque key defeats the point of an
  explicit `StateKey` and makes stored state undebuggable. Local-storage keys have no character
  restrictions, so the encoding bought nothing.
- A `SelectedChanged` callback or two-way binding on `Selected`. Neither issue asks for it.

## Consumer impact

`AllowSaveState` defaults to `true`, so on upgrade **every existing `ExpandableCard` starts remembering
its expanded state**. That is the documented contract finally taking effect, but it is a visible
behaviour change: a card declared `Selected="true"` will now open collapsed if the user last collapsed
it. This lands in the 2.3 series (already started in `8d16d4f`), so a minor bump carries it.

## Acceptance criteria

- [ ] With `AllowSaveState="true"`, collapsing a card and re-mounting it restores the collapsed state.
- [ ] The stored key is `ExpandableCard.{StateKey}.Selected` when `StateKey` is set, and
      `ExpandableCard.{Text}.Selected` when it is not.
- [ ] A `Text` that changes after the first render does not change the key the component reads or writes.
- [ ] On an interactive renderer the card's first paint already shows the stored state — the stored
      value is read before any render, not after.
- [ ] `AllowSaveState="false"` neither reads nor writes storage.
- [ ] Two cards with different `StateKey` values but identical `Text` do not share state.
- [ ] Full test suite passes; docs and README describe the parameter set as implemented.

## Done condition

Both #15 and #16 are closed with the shipped evidence, docs updated on both surfaces, PR open against
`master`.
