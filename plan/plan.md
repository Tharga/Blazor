# Plan: datetimeview-date-display

## Steps
- [x] 1. NuGet update — `dotnet outdated` reports nothing; no dependency commit needed.
- [~] 2. Tests first for `EDateTimeDisplay.Date`: text, tooltip, Swedish tooltip, no timer, null date
- [ ] 3. Implement: enum member, `Text`/`Tooltip` via a switch, `EffectiveInterval` skips `Date`; update enum docs
- [ ] 4. Drop `Microsoft.NET.Test.Sdk` and `xunit.runner.visualstudio`; verify test count unchanged
- [ ] 5. Docs: README + `docs/articles/datetimeview.md`
- [ ] 6. Close-out

## Notes
- Branch `feature/datetimeview-date-display` from `master` (GitHub Actions → PR to master).

## Last session
2026-09-27 — branch created, plan written. Next: step 2.
