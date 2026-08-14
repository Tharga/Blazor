# Plan: CustomErrorBoundary access denial + stack-trace leak (#14)

## Steps

- [x] 1. NuGet package check (mandatory feature-start step)
      Done: `dotnet outdated` reports no outdated dependencies across the solution. Nothing to apply.

- [x] 2. Establish why the issue's suggested guard cannot be used as written
      Done: `project.assets.json` shows `Microsoft.NETCore.App` as the only framework reference, so
      neither `IWebHostEnvironment` nor `IHostEnvironment` is available, and neither is registered on
      Blazor WebAssembly regardless. Recorded in `plan/feature.md`; decision confirmed with the user.

- [x] 3. Add `BlazorOptions.ShowExceptionDetails`
      Done: `bool?`, `null` reads as off, XML docs explaining why the library cannot decide for itself.
      `AddThargaBlazor` applies it behind the same `if (… != null)` guard `Title` uses.

- [x] 4. Write the tests first
      Done: `CustomErrorBoundaryTests` (13) + `AddThargaBlazorShowExceptionDetailsTests` (4), with
      `ThrowingComponent`, `FakeLogger<T>` and `FakeErrorBoundaryLogger` as the doubles. Two notes for
      anyone extending these: the boundary's crash panel renders `CopyButton`, so the test context
      needs `Services.AddRadzenComponents()` or `NotificationService` fails to resolve mid-render; and
      the built-in `ErrorBoundary` injects `IErrorBoundaryLogger`, which nothing registers in bUnit.

- [x] 5. Implement the three render paths and the log-level split in `CustomErrorBoundary.razor`
      Done. The stack-trace assertions key on the rendered trace containing `ThrowingComponent`, which
      is what makes the on/off pair a real guard rather than two tests that agree with each other.

- [x] 6. Run the full suite in Release
      Done: 56 passed, 0 failed (39 existing + 17 new).

- [~] 7. Docs (`docs:` commit)
      New `docs/articles/errorboundary.md` + `docs/articles/toc.yml` entry — the component has no
      article today. Update the README's *Error Handling* section, which currently shows only the
      wrapper markup and does not mention that stack traces are involved at all.

- [ ] 8. Commit, push the branch, ask the user to test. Do not open the PR yet.

- [ ] 9. On user confirmation: re-check `dotnet outdated`, close #14 with evidence, archive
      `plan/feature.md` to the Plan directory, `git rm -r plan`, final `fix:` commit, open the PR.

## Notes and decisions

- **Access-denied panel shape** (confirmed with the user): `Access denied` label, exception message,
  `Try again` button, in an `access-denied-content` class so hosts can style it apart from the crash
  panel. No CorrelationId line — it is logged at Warning, and offering a correlation id invites a bug
  report for something that is not a bug.

- **The dev guard is a host-controlled option, not auto-detection** (confirmed with the user). See
  `plan/feature.md` for why auto-detection is not available to this library. Fails closed.

- **`ErrorContent` now replaces the whole panel.** Not named in the issue, but the heading currently
  renders above host-supplied content, which makes the documented `ErrorBoundary` extension point
  useless for the exact case the issue is about. Fixed here rather than filed separately.

- **Version.** Next CI version is 2.3.2 and no public API breaks. The stack-trace default flip is
  visible behaviour though — decide at close-out whether it rides a patch with release notes or
  warrants raising `MAJOR_MINOR` to 2.4.

## Last session

2026-08-14 — Branch `fix/errorboundary-access-denied` created off `master` (which is current, carrying
the merged #15/#16 work). Plan confirmed with the user on the two material decisions.

2026-08-15 — Steps 1-6 done: option, three render paths, log-level split, 17 new tests, suite green at
56. Next: step 7, the docs commit.
