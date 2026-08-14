# Feature: CustomErrorBoundary recognises access denial and stops leaking stack traces (GitHub #14)

## Goal

Stop `CustomErrorBoundary` presenting an expected authorization failure as an application crash, and
stop it rendering stack traces in production.

## The two defects

1. **Every exception renders identically.** `UnauthorizedAccessException` from a
   `[RequireAccessLevel]`-gated call produces a full-page "Something went wrong!" panel with a stack
   trace dump and a "Try to recover" button. Consumers commonly wrap all content in the boundary from
   `MainLayout`, so this is what a user sees for a routine permission outcome.
2. **Stack traces render everywhere, including production.** The `IsDevelopment()` guard around the
   stack-trace block is commented out (`CustomErrorBoundary.razor:24-36`), together with the
   `IWebHostEnvironment` injection it needed (line 7).

## Why the issue's suggested guard cannot be restored as written

`@inject IWebHostEnvironment` was commented out for a reason that is still true: **this project has no
ASP.NET Core framework reference.** `project.assets.json` lists `Microsoft.NETCore.App` only — the
Components types arrive as ordinary NuGet packages via Radzen.Blazor. `IWebHostEnvironment` lives in
`Microsoft.AspNetCore.Hosting.Abstractions`, which is not referenced, and `IHostEnvironment` is in
`Microsoft.Extensions.Hosting.Abstractions`, which is not either.

Adding one would compile, but it would still be wrong: **neither service is registered on Blazor
WebAssembly**, which uses `IWebAssemblyHostEnvironment` instead. A library that auto-detects would
silently decide "not development" on every WASM host and be unable to say so.

So the host decides, which it always can:

- **New `BlazorOptions.ShowExceptionDetails`, off unless set** — fails closed, which is the half of
  this issue that is a production information leak. Typed `bool?` rather than `bool`, for the same
  reason `Title` is a reference type: `AddThargaBlazor` applies the code callback *after* the
  configuration binding and can only let code win where it can tell "set" from "left alone". A
  non-nullable `bool` would make an unset callback clobber `"ShowExceptionDetails": true` from
  `appsettings.Development.json` with its own `false`. `null` reads as off.
- Set in code — `AddThargaBlazor(o => o.ShowExceptionDetails = builder.Environment.IsDevelopment())` —
  or per environment in `appsettings.Development.json` under the `Tharga:Blazor` section that
  `AddThargaBlazor` already binds. No new dependency, correct on both render models.

## Scope

- `BlazorOptions.ShowExceptionDetails` (`bool?`, off unless set), wired through `AddThargaBlazor` the
  same way `Title` is.
- `CustomErrorBoundary` renders three ways instead of one:
  - **`ErrorContent` supplied** → render it alone. Today the "Something went wrong!" heading renders
    *above* host-supplied content, so a host cannot actually take over the panel. Same defect family,
    fixed here.
  - **`UnauthorizedAccessException`** → compact panel: an `Access denied` label, the exception message,
    a `Try again` button. No crash heading, no stack trace, no CorrelationId line. Logged at
    **Warning**, not Error.
  - **Anything else** → today's panel, with the stack-trace block gated on `ShowExceptionDetails`.
- bUnit tests for all three paths and both log levels.
- Docs: a new `docs/articles/errorboundary.md` (the component has no article), plus the README's
  existing *Error Handling* section.

## Out of scope

- Unwrapping inner exceptions to find a nested `UnauthorizedAccessException`. Blazor hands the boundary
  the thrown exception, and gated service calls throw it directly.
- Making the access-denied strings localizable. `Tharga.Team.Blazor` has `IThargaTextProvider` for
  this; introducing a second mechanism here is a larger decision than this issue.
- Shipping CSS for `.error-content` / `.access-denied-content`. The library ships no stylesheet for the
  existing classes either — styling is the host's.

## Consumer impact

**Stack traces stop rendering by default.** A developer who relied on seeing them locally must set
`ShowExceptionDetails` once, in code or in `appsettings.Development.json`. That is the intended
direction: the current default leaks them to end users in production.

**Ships as 2.3.2** — decided 2026-08-15, `MAJOR_MINOR` stays at `2.3`. Nothing in the public API
breaks (the new option is additive) and the behaviour change is a security fix rather than a
capability change, so it rides the patch. The default flip still gets said out loud in the release
notes and in the issue close-out, since a developer who loses their local stack traces will otherwise
read it as a regression.

## Acceptance criteria

- [ ] `UnauthorizedAccessException` renders the access-denied panel: no "Something went wrong!", no
      stack trace, no CorrelationId, and the exception message is shown.
- [ ] `UnauthorizedAccessException` is logged at `Warning`; every other exception at `Error`.
- [ ] With `ShowExceptionDetails` unset, no exception type renders a stack trace.
- [ ] With `ShowExceptionDetails = true`, a non-authorization exception renders message and stack trace.
- [ ] `ShowExceptionDetails` binds from `Tharga:Blazor` configuration and from the code callback, code
      winning, matching `Title`'s existing behaviour.
- [ ] Host-supplied `ErrorContent` renders alone, with no crash heading above it.
- [ ] Every exception still logs a CorrelationId and still attaches it to the exception data.
- [ ] Full suite green; docs updated on both surfaces.

## Done condition

#14 closed with the shipped evidence, docs on both surfaces, PR open against `master`.
