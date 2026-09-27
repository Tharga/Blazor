# Plan: datetimeview-date-display

## Steps
- [x] 1. NuGet update — `dotnet outdated` reports nothing; no dependency commit needed.
- [x] 2. Tests: text, tooltip, Swedish tooltip, null date (empty tooltip, not a lone " · "), no timer. 4 new.
- [x] 3. `EDateTimeDisplay.Date` appended; `Text`/`Tooltip` are switches; refresh now runs only for `Relative`
      (was "not Absolute"). Enum and `RefreshInterval` docs updated. 126/126.
- [x] 4. Dropped both packages. `Microsoft.NET.Test.Sdk` had been supplying `OutputType=Exe`, which xunit v3
      requires, so it is now declared in the csproj. 126/126, and the CI coverage command still produces cobertura.
- [x] 5. Docs: `datetimeview.md` section renamed "Relative, absolute or date", table row, rule, refresh note, parameter table; README component line and staleness note.
- [~] 6. Close-out

## Notes
- Branch `feature/datetimeview-date-display` from `master` (GitHub Actions → PR to master).

## Last session
2026-09-27 — branch created, plan written. Next: step 2.
