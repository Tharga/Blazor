# Feature: move-auth-to-team-blazor

**Originating branch:** develop
**Date started:** 2026-03-19

## Goal

Move auth registration helpers (`AddThargaAuth`/`UseThargaAuth`, `ThargaAuthOptions`) from Tharga.Blazor to Tharga.Team.Blazor so auth code lives alongside the auth UI components (`LoginDisplay`, `UserProfileView`).

## Scope

- Move `ThargaAuthOptions.cs` and `ThargaAuthRegistration.cs` to Tharga.Team.Blazor
- Move `Microsoft.Identity.Web` dependency to Tharga.Team.Blazor
- Remove `FrameworkReference Microsoft.AspNetCore.App` from Tharga.Blazor (if no longer needed)
- Move tests to Tharga.Team.Blazor.Tests
- Restore Tharga.Blazor README to generic UI-only description
- Update Tharga.Team.Blazor README with auth docs
- Update Implementation Guide

## Acceptance criteria

- [ ] Auth code and UI components are both in Tharga.Team.Blazor
- [ ] Tharga.Blazor has no auth or Microsoft.Identity.Web dependency
- [ ] All tests pass
- [ ] READMEs updated for both packages
- [ ] Implementation Guide updated

## Done condition

All acceptance criteria met, all tests pass, user confirms feature is complete.
