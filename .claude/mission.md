# Mission: Tharga.Platform

Shared Blazor components and team management infrastructure, published as NuGet packages (Tharga.Blazor, Tharga.Team.Blazor, Tharga.Team.Service, Tharga.Team.MongoDB).

## Change Request: Simplify Registration and Improve Error Resilience

**Requested by:** Daniel Bohlin (Quilt4Net Server, PlutusWave — adoption of Platform 2.0.x)
**Date:** 2026-03-23

### Problem

Adopting Tharga Platform requires too many separate `Add*` calls, and when one is missing the app either crashes silently at startup or renders a blank page with no guidance. Three categories of issues were hit during Quilt4Net Server migration:

#### 1. Too many registration calls required
A consumer currently needs to call up to 7 separate methods: `AddThargaAuth()`, `AddThargaApiKeyAuthentication()`, `AddThargaApiKeys()`, `AddThargaTeamBlazor()`, `AddThargaTeamRepository()`, `AddThargaScopes()`, `AddThargaTenantRoles()`. Missing any one causes a runtime crash. There is no single entry point that sets up sensible defaults.

#### 2. Missing services crash without useful feedback
- `TeamComponent` has `[Inject] ITenantRoleRegistry` — if `AddThargaTenantRoles()` hasn't been called, the page crashes with an unhandled `InvalidOperationException`. The user sees a blank page; the error only appears in the browser console via `blazor.web.js`.
- `ApiKeyView` requires `IApiKeyManagementService` — not registered by `AddThargaApiKeys()`.
- `IScopeRegistry` was a hard `[Inject]` dependency (fixed in 2.0.1-pre.2 to null-safe, but the same pattern is not applied everywhere).

#### 3. MongoDB auto-registration silently misses Platform assemblies
`AddMongoDB()` scans assemblies by entry-assembly name prefix (e.g. `Quilt4Net`). `Tharga.Team.Service` doesn't match, so `IApiKeyRepository` and `IApiKeyRepositoryCollection` are never auto-registered. The consumer must know to call `o.AddAutoRegistrationAssembly(typeof(ApiKeyConstants).Assembly)` — this is undocumented and the resulting `AggregateException` at startup gives no hint about assembly scanning.

### Goal

Make it simple to adopt Platform with as few `Add*` registrations as possible, with sensible defaults that work out of the box. When something is misconfigured, provide clear in-context error messages instead of crashes.

### Requirements

1. **Provide a single top-level registration** (e.g. `AddThargaPlatform()`) that sets up all core services with default options. Individual `Add*` methods remain available for advanced/partial use.

2. **All services required by Blazor components must be registered by the same call that makes those components available.** If `AddThargaTeamBlazor()` exposes `TeamComponent` and `ApiKeyView`, it must also register their dependencies — or make them optional/null-safe.

3. **Graceful degradation for optional services.** Components should use `IServiceProvider.GetService<T>()` instead of `[Inject]` for optional dependencies (`ITenantRoleRegistry`, `IScopeRegistry`). When a required service is truly missing, render a clear in-page error message (e.g. "Tenant roles are not configured. Call `AddThargaTenantRoles()` in Program.cs.") instead of crashing.

4. **Platform packages should register their own assemblies for MongoDB auto-scanning.** `AddThargaApiKeys()` or `AddThargaTeamRepository()` should call `AddAutoRegistrationAssembly` for `Tharga.Team.Service` so consumers don't have to. The current behavior silently fails when the entry assembly has a different name prefix.

5. **Document the minimal registration path** in each package's README — one call to get started, with examples of customization.

### Context

Discovered during Quilt4Net Server migration to Platform 2.0.1-pre.1. These same issues will affect PlutusWave, FortDocs Web, and any other project adopting Platform. Each project currently needs to independently discover the correct combination of registration calls and workarounds.

## External References
- **Backlog**: `c:\Users\danie\SynologyDrive\Documents\Notes\Tharga\Toolkit\Platform.md`
