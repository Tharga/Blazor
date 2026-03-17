# Plan: Restructure into Tharga.Platform monorepo

## Goal
Rename the repo to Tharga.Platform, split Tharga.Blazor into generic and team-specific packages, and move Tharga.Api into the same repo for easier maintenance.

## Target package structure
```
Tharga.Platform (repo)
├── Tharga.Team              — Domain models, authorization, service abstractions (plain .NET)
├── Tharga.Team.MongoDB      — MongoDB persistence for teams/users
├── Tharga.Team.Blazor       — Team-specific Blazor components (NEW)
├── Tharga.Blazor            — Generic Blazor UI components (slimmed down)
├── Tharga.Api               — Server-only API auth, Swagger, audit (MOVED from separate repo)
├── Tharga.Blazor.Tests      — Tests for generic components
├── Tharga.Team.Blazor.Tests — Tests for team components (NEW)
└── Tharga.Api.Tests          — Tests for API (MOVED from separate repo)
```

## Dependency graph
```
Tharga.Team                          (plain .NET)
     ↑           ↑            ↑
Tharga.Blazor  Tharga.Team.MongoDB  Tharga.Api
(generic UI)   (+ Tharga.MongoDB)   (+ Tharga.MongoDB, ASP.NET Core, Swagger)
     ↑
Tharga.Team.Blazor
(+ Tharga.Team)
```

## What goes where

### Tharga.Blazor (generic — no team dependencies)
- Features/BreadCrumbs/
- Framework/Buttons/ (ActionButton, CancelButton, CopyButton, StandardButton)
- Features/Common/ (DateTimeView, TimeSpanView, ExpandableCard, Loading, Title)
- Framework/CustomErrorBoundary

### Tharga.Team.Blazor (team-specific)
- Features/Team/ (TeamSelector, TeamComponent, TeamDialog, InviteUserDialog, TeamInviteView, TeamStateService, ITeamStateService, UserProfileView)
- Features/Api/ (ApiKeyView, ApiKeyModel)
- Framework/TeamClaimsAuthenticationStateProvider
- Framework/ThargaBlazorRegistration (→ renamed to ThargaTeamBlazorRegistration)
- Framework/ThargaBlazorOptions
- Framework/LoginDisplay
- Framework/Roles
- Framework/Constants

## Steps

### Phase 1: Move Tharga.Api into the repo
- [ ] **1.1** Copy Tharga.Api source and test projects from `C:\dev\tharga\Toolkit\Api` into the Blazor repo.
- [ ] **1.2** Update Tharga.Api.csproj — replace NuGet Tharga.Team reference with project reference, remove `99.0.0-local` version.
- [ ] **1.3** Update solution file to include Tharga.Api and Tharga.Api.Tests.
- [ ] **1.4** Build and test.

### Phase 2: Create Tharga.Team.Blazor project
- [ ] **2.1** Create Tharga.Team.Blazor.csproj (Razor SDK, targets net9.0;net10.0).
- [ ] **2.2** Move team-specific files from Tharga.Blazor to Tharga.Team.Blazor.
- [ ] **2.3** Update namespaces — `Tharga.Blazor.*` → `Tharga.Team.Blazor.*` where appropriate.
- [ ] **2.4** Add project references: Tharga.Team.Blazor → Tharga.Blazor + Tharga.Team.
- [ ] **2.5** Create Tharga.Team.Blazor.Tests with moved tests.

### Phase 3: Slim down Tharga.Blazor
- [ ] **3.1** Remove team-specific files from Tharga.Blazor (already moved in Phase 2).
- [ ] **3.2** Remove Tharga.Team dependency from Tharga.Blazor.csproj.
- [ ] **3.3** Verify Tharga.Blazor has no team/auth imports.

### Phase 4: Update CI/CD pipeline
- [ ] **4.1** Update azure-pipelines.yml to build, pack, and push all 5 packages.
- [ ] **4.2** Add pack/push steps for Tharga.Api and Tharga.Team.Blazor.
- [ ] **4.3** Ensure correct build order (Team → Blazor → Api, Team.MongoDB, Team.Blazor).

### Phase 5: Documentation
- [ ] **5.1** Update top-level README.md for Tharga.Platform.
- [ ] **5.2** Update Tharga.Blazor/README.md — generic components only.
- [ ] **5.3** Create Tharga.Team.Blazor/README.md.
- [ ] **5.4** Verify Tharga.Api/README.md is current.
- [ ] **5.5** Update Tharga.Team/README.md if needed.

### Phase 6: Version bump and cleanup
- [ ] **6.1** Bump majorMinor in azure-pipelines.yml.
- [ ] **6.2** Remove `local-packages/` and `_removed_audit/` folders.
- [ ] **6.3** Build all projects.
- [ ] **6.4** Run all tests.

## Notes
- The repo rename (Blazor → Platform) happens on GitHub — this plan covers the content restructuring.
- Tharga.Api.csproj switches from NuGet to project reference for Tharga.Team.
- AuditLogView (previously removed) can be added back to Tharga.Api or Tharga.Team.Blazor if needed later.
