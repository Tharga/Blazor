# Feature: Split Blazor Registration

## Goal
Extract basic Blazor services from `Tharga.Team.Blazor` into a standalone `AddThargaBlazor()` extension in `Tharga.Blazor`, and rename the team-level registration to `AddThargaTeamBlazor()`.

## Originating branch
develop

## Scope
- Create `AddThargaBlazor(Action<BlazorOptions>)` in `Tharga.Blazor` with BreadCrumbService, BlazoredLocalStorage, and BlazorOptions configuration
- Support appsettings.json binding with code-override-wins
- Make `ThargaBlazorOptions` inherit from `BlazorOptions`
- Rename `AddThargaBlazor` → `AddThargaTeamBlazor` in `Tharga.Team.Blazor`
- `AddThargaTeamBlazor` calls `AddThargaBlazor` internally
- Update all tests
- Update documentation (README, implementation guide)
- Minor version bump (breaking change)

## Acceptance criteria
- [ ] `Tharga.Blazor` exposes `AddThargaBlazor(Action<BlazorOptions>)` that registers BreadCrumbService, BlazoredLocalStorage, and BlazorOptions
- [ ] `BlazorOptions` supports binding from `IConfiguration` section `Tharga:Blazor`
- [ ] `ThargaBlazorOptions` inherits `BlazorOptions`
- [ ] `AddThargaTeamBlazor` in `Tharga.Team.Blazor` calls `AddThargaBlazor` internally
- [ ] Old `AddThargaBlazor` name no longer exists in `Tharga.Team.Blazor`
- [ ] All existing tests pass
- [ ] New tests cover the new registration path
- [ ] README and implementation guide updated
- [ ] Minor version bumped

## Done condition
All acceptance criteria met, all tests pass, user confirms done.
