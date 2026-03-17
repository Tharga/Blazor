# Plan: Restructure package dependencies for WASM compatibility

## Goal
Make `Tharga.Blazor` usable from pure WASM projects by eliminating the transitive dependency on server-only packages (`Microsoft.AspNetCore.OpenApi`, `Swashbuckle`, `Tharga.MongoDB`).

## Current dependency chain (broken for WASM)
```
Tharga.Blazor → Tharga.Team → Tharga.Api → Microsoft.AspNetCore.OpenApi, Swashbuckle, Tharga.MongoDB
```

## Target dependency chain
```
WASM project:    Tharga.Blazor → Tharga.Team (plain .NET only)
Server project:  Tharga.Blazor → Tharga.Team.MongoDB → Tharga.Team + Tharga.MongoDB
                                + Tharga.Api (directly, for auth handler / Swagger / audit)
```

## What moves where

### Types moving FROM Tharga.Api INTO Tharga.Team (plain .NET, WASM-safe)
These are team/tenant concepts with no server dependencies:
- `AccessLevel` enum
- `TeamClaimTypes` (claim type constants)
- `TeamScopes`, `ApiKeyScopes` (scope constants)
- `IScopeRegistry` / `ScopeRegistry` / `ScopeDefinition`
- `ITenantRoleRegistry` / `TenantRoleRegistry` / `TenantRoleDefinition`
- `RequireAccessLevelAttribute` / `RequireScopeAttribute`
- `AccessLevelProxy<T>` / `ScopeProxy<T>` (DispatchProxy — plain .NET)
- `IApiKey`, `IApiKeyAdministrationService`, `IApiKeyManagementService` (interfaces only)
- `ApiKeyOptions`
- DI extensions: `AccessLevelServiceCollectionExtensions`, `ScopeServiceCollectionExtensions`, `TenantRoleServiceCollectionExtensions`

### Types staying in Tharga.Api (server-only)
These require ASP.NET Core, MongoDB, or Swagger:
- `ApiKeyAuthenticationHandler` (ASP.NET Core auth handler)
- `ApiKeyEntity` / `ApiKeyAdministrationService` / `ApiKeyManagementService` (MongoDB implementations)
- `ApiKeyRepository` / `ApiKeyRepositoryCollection` (MongoDB)
- `ApiKeyRegistration` (DI for auth handler)
- `ApiKeyConstants` (header name, scheme name)
- `ControllersRegistration` / `ThargaControllerOptions` (Swagger)
- Entire `Audit/` folder (~18 files — ASP.NET Core + MongoDB)

### Tharga.Api changes
After migration, Tharga.Api:
- Adds a dependency on `Tharga.Team` (for `AccessLevel`, `IApiKey`, scopes, etc.)
- Removes `Tharga.Toolkit` dependency (moves to Tharga.Team if needed)
- Keeps `Tharga.MongoDB`, `Swashbuckle`, `Microsoft.AspNetCore.OpenApi`

### Tharga.Team changes
- Adds the moved types (see above)
- Removes dependency on `Tharga.Api`
- Keeps `Microsoft.AspNetCore.Components.Authorization` (WASM-safe)
- Adds `Tharga.Toolkit` dependency (if needed by moved types)

## Steps

### Phase 1: Move types from Tharga.Api to Tharga.Team
- [ ] **1.1** Create folders/namespaces in Tharga.Team for the moved types. Use a namespace like `Tharga.Team` (not `Tharga.Api`) for new home.
- [ ] **1.2** Move the plain .NET types listed above from Tharga.Api to Tharga.Team.
- [ ] **1.3** Update `Tharga.Team.csproj` — remove `Tharga.Api` dependency, add `Tharga.Toolkit` if needed.
- [ ] **1.4** Move or create corresponding unit tests in the Blazor test project.

### Phase 2: Update Tharga.Api to depend on Tharga.Team
- [ ] **2.1** Add `Tharga.Team` as a dependency to `Tharga.Api.csproj`.
- [ ] **2.2** Remove the moved source files from Tharga.Api.
- [ ] **2.3** Add `[Obsolete]` type-forwarding shims in Tharga.Api for the moved types (to avoid breaking existing consumers on upgrade). These forward to the types now in Tharga.Team.
- [ ] **2.4** Update Tharga.Api tests — adjust imports/namespaces.
- [ ] **2.5** Build and test Tharga.Api.

### Phase 3: Update Tharga.Blazor
- [ ] **3.1** Update `Tharga.Blazor.csproj` — verify it only depends on `Tharga.Team` (no `Tharga.Api`).
- [ ] **3.2** Update imports in Tharga.Blazor source files (namespaces may have changed for moved types).
- [ ] **3.3** Build and test Tharga.Blazor.

### Phase 4: Update Tharga.Team.MongoDB
- [ ] **4.1** Verify `Tharga.Team.MongoDB` depends on `Tharga.Team` + `Tharga.MongoDB` (no `Tharga.Api`).
- [ ] **4.2** Update imports if needed.
- [ ] **4.3** Build.

### Phase 5: Validate WASM compatibility
- [ ] **5.1** Build `Quilt4Net.Server.Client` (WASM project) with the updated local packages.
- [ ] **5.2** Verify the `NETSDK1082 browser-wasm` error is resolved.
- [ ] **5.3** Build the full `Quilt4Net.Server` solution to confirm server-side still works.

### Phase 6: Version bumps and publish
- [ ] **6.1** Bump Tharga.Team version (minor bump — new public types).
- [ ] **6.2** Bump Tharga.Api version (minor bump — breaking: types moved, shims added).
- [ ] **6.3** Bump Tharga.Blazor version if needed.
- [ ] **6.4** Publish packages in order: Tharga.Team → Tharga.Api → Tharga.Team.MongoDB → Tharga.Blazor.

## Risks and considerations
- **Breaking change for Tharga.Api consumers**: Types move namespaces. Mitigated by `[Obsolete]` type-forwarding in Phase 2.3. Consider a major version bump for Tharga.Api if clean break is preferred.
- **Namespace decisions**: Moved types currently live in `Tharga.Api` namespace. Options: (a) keep `Tharga.Api` namespace via `[TypeForwardedTo]`, (b) move to `Tharga.Team` namespace. Option (b) is cleaner long-term but requires consumers to update `using` statements.
- **Tharga.Api is a separate repo** (`C:\dev\tharga\Toolkit\Api`): Changes span two repositories (Blazor + Api). Coordinate releases.
- **Publish order matters**: Tharga.Team must be published before Tharga.Api (since Api will depend on Team).
