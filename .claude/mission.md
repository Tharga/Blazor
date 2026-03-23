# Mission: Tharga.Platform

Shared Blazor components and team management infrastructure, published as NuGet packages (Tharga.Blazor, Tharga.Team.Blazor, Tharga.Team.Service, Tharga.Team.MongoDB).

## Documentation Requests
- **Rate limiting configuration guide** — The Starter project (https://github.com/Tharga/Starter) references Platform for rate limiting docs, but none exist. Add documentation covering: default values, how to customize PermitLimit/Window, available rate limiter types (FixedWindow, SlidingWindow, TokenBucket), and integration with audit logging. Requested by: Tharga.Starter.

- **SSR deadlock warning and migration guide for Step 4 (Team Management)** — The implementation guide (`docs/implementation-guide.md`) does not mention that `AddThargaTeamBlazor()` causes a silent SSR deadlock in Blazor apps using server-side rendering. The `TeamClaimsAuthenticationStateProvider` decorator uses JS interop (localStorage) which is unavailable during SSR prerendering, causing the page to hang indefinitely (white screen, no errors). This has been hit by multiple projects. The guide needs the following updates:

  **In Step 4 (Team Management):**
  1. Add an "SSR Compatibility" section explaining that apps using `AddInteractiveServerComponents()` MUST set `o.SkipAuthStateDecoration = true` in `AddThargaTeamBlazor` options (available since 2.0.1-pre.1).
  2. Document that when `SkipAuthStateDecoration = true`, the `team_id` claim is no longer added automatically. Apps must register an `IClaimsTransformation` that reads the `selected_team_id` cookie and adds the `team_id` claim server-side.
  3. Provide the full `TeamCookieClaimsTransformation` class as a ready-to-use example.
  4. Warn that without the claims transformation, `TeamStateService.GetSelectedTeamAsync()` enters an infinite refresh loop (`_navigationManager.Refresh(true)` → no claim found → repeat).
  5. Update the "Quick reference: Registration order" section to include `SkipAuthStateDecoration = true` and the `IClaimsTransformation` registration.

  **In Step 2 (Authentication):**
  6. Note that `UseThargaAuth()` requires >= 2.0.1-pre.1 for correct async login behavior. Version 2.0.0 used `Results.Challenge` (sync) which caused DNS errors with some Azure AD configurations.

  Requested by: Quilt4Net Server (`C:\dev\tharga\Quilt4Net`) and PlutusWave (`C:\dev\tharga\PlutusWave`). Both projects hit the white screen issue during platform migration.

- **Graceful error when `ITenantRoleRegistry` is not registered** — When `TeamComponent` is rendered but `AddThargaTenantRoles()` has not been called, the app crashes with an unhandled `InvalidOperationException` ("no registered service of type 'Tharga.Team.ITenantRoleRegistry'"). Instead of crashing, the component should display an in-page error message (e.g. "Tenant roles are not configured. Call AddThargaTenantRoles() in Program.cs.") so the developer gets clear guidance without a full page failure. Requested by: Quilt4Net Server (`C:\dev\tharga\Quilt4Net`).

## External References
- **Backlog**: `c:\Users\danie\SynologyDrive\Documents\Notes\Tharga\Toolkit\Platform.md`
